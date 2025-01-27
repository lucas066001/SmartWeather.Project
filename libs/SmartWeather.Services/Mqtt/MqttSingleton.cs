namespace SmartWeather.Services.Mqtt;

using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Options;

public class MqttSingleton
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _mqttOptions;
    private List<IMqttRequestHandler> _requestHandlers;
    private List<IMqttMessageHandler> _messageHandlers;
    internal record MqttPendingRequest
    {
        public required MqttHeader OriginalRequestHeader { get; set; }
        public required TaskCompletionSource<MqttResponse> TaskSource { get; set; }
    }

    internal List<MqttPendingRequest> PendingRequestList;

    public MqttSingleton(IServiceScopeFactory scopeFactory)
    {
        _requestHandlers = new List<IMqttRequestHandler>();
        _messageHandlers = new List<IMqttMessageHandler>();
        _mqttClient = new MqttFactory().CreateMqttClient();
        PendingRequestList = new List<MqttPendingRequest>();

        IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

        // Create a new MQTT client

        var clientId = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.ClientId)];
        var brokerAddress = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.BrokerAddress)];
        int.TryParse(configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.BrokerPort)], out int brokerPort);
        var username = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.Username)];
        var password = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.Password)];

        _mqttOptions = new MqttClientOptionsBuilder()
            .WithClientId($"{clientId}.{Guid.NewGuid()}")
            .WithTcpServer(brokerAddress, brokerPort) 
            .WithCleanSession()
            .WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
            .WithTlsOptions(new MqttClientTlsOptions
            {
                UseTls = false,
                AllowUntrustedCertificates = true
            })
            .WithKeepAlivePeriod(new TimeSpan(0,5,0))
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += MessageHandler;
    }

    public async Task ConnectAsync()
    {
        if (!_mqttClient.IsConnected)
        {
            await _mqttClient.ConnectAsync(_mqttOptions);
        }
    }

    public async Task SubscribeAsync(string topic)
    {
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
        }
    }

    public async Task DisconnectAsync()
    {
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync();
        }
    }

    public async Task SendErrorResponse(MqttRequest? request, string originTopic, string errorMessage = BaseResponses.INTERNAL_ERROR, Status status = Status.INTERNAL_ERROR)
    {
        MqttHeader errorHeader;

        if (request != null)
        {
            errorHeader = new MqttHeader()
            {
                RequestEmitter = request.Header.RequestEmitter,
                RequestIdentifier = request.Header.RequestIdentifier
            };
        }
        else
        {
            errorHeader = new MqttHeader()
            {
                RequestEmitter = DefaultIdentifiers.DEFAULT_SENDER_ID,
                RequestIdentifier = DefaultIdentifiers.DEFAULT_REQUEST_ID
            };
        }

        var errorResponse = MqttResponse.Failure(errorHeader,
                                                 string.Format(BaseResponses.FORMAT_ERROR, errorMessage), 
                                                 status);
        var responseTopic = originTopic.Replace(CommunicationConstants.MQTT_SERVER_TARGET,
                                                CommunicationConstants.MQTT_STATION_TARGET);

        await PublishAsync(responseTopic, JsonSerializer.Serialize(errorResponse));
    }

    public async Task SendSuccessResponse(MqttRequest request, string originTopic, ObjectTypes objectType, Object data)
    {
        var successHeader = new MqttHeader()
        {
            RequestEmitter = request.Header.RequestEmitter,
            RequestIdentifier = request.Header.RequestIdentifier
        };

        var successResponse = MqttResponse.Success(successHeader, (int)objectType, data);
        var responseTopic = originTopic.Replace(CommunicationConstants.MQTT_SERVER_TARGET,
                                                CommunicationConstants.MQTT_STATION_TARGET);

        await PublishAsync(responseTopic, JsonSerializer.Serialize(successResponse));
    }

    public async Task SendRequest(MqttHeader requestHeader, string targetTopic, ObjectTypes objectType, Object data)
    {
        string jsonObject = JsonSerializer.Serialize(data);

        var serverRequest = new MqttRequest(requestHeader,
                                            jsonObject, 
                                            jsonObject.Length,
                                            (int)objectType);

        await PublishAsync(targetTopic, JsonSerializer.Serialize(serverRequest));

        return;
    }

    public void RegisterRequestHandler(IMqttRequestHandler handler)
    {
        _requestHandlers.Add(handler);
    }

    public void RegisterMessageHandler(IMqttMessageHandler handler)
    {
        _messageHandlers.Add(handler);
    }

    public async Task<T?> RetreiveMqttObject<T>(string message, string topic, MqttRequest? request = null, bool automaticMqttError = true) where T : class
    {
        T? objectFound = null;

        try
        {
            objectFound = JsonSerializer.Deserialize<T>(message);
        }
        catch (Exception ex)
        {
            if (automaticMqttError)
            {
                await SendErrorResponse(request, topic, "Unable to deserialize Mqtt object : " + ex.Message, Status.PARSE_ERROR);
            }
        }

        return objectFound;
    }

    public async Task PublishAsync(string topic, string payload)
    {
        if (!_mqttClient.IsConnected)
        {
            throw new InvalidOperationException("MQTT client is not connected.");
        }

        var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag(false)
            .Build();

        await _mqttClient.PublishAsync(mqttMessage);
    }

    private async Task MessageHandler(MqttApplicationMessageReceivedEventArgs e)
    {
        foreach (var handler in _messageHandlers)
        {
            if (handler.IsAbleToHandle(e.ApplicationMessage.Topic))
            {
                handler.Handle(e.ApplicationMessage.ConvertPayloadToString(), e.ApplicationMessage.Topic);
                return;
            }
        }

        var response = await RetreiveMqttObject<MqttResponse>(e.ApplicationMessage.ConvertPayloadToString(),
                                                              e.ApplicationMessage.Topic,
                                                              null,
                                                              false);

        if (response != null && ContractHelper.IsResponseType(response.JsonType))
        {
            var pendingRequest = PendingRequestList.Where(pr => pr.OriginalRequestHeader.RequestIdentifier == response.Header.RequestIdentifier).FirstOrDefault();

            if (pendingRequest != null)
            {
                pendingRequest.TaskSource.SetResult(response);
                PendingRequestList.Remove(pendingRequest);
            }
            
            return;
        }

        var request = await RetreiveMqttObject<MqttRequest>(e.ApplicationMessage.ConvertPayloadToString(),
                                                            e.ApplicationMessage.Topic,
                                                            null,
                                                            false);


        if (request != null && ContractHelper.IsRequestType(request.JsonType))
        {
            foreach (var handler in _requestHandlers)
            {
                if (handler.IsAbleToHandle(request.JsonType))
                {
                    handler.Handle(request, e.ApplicationMessage.Topic);
                    break;
                }
            }
        }
        return;
    }
}