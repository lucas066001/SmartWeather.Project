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
    internal record MqttPendingRequest
    {
        public required MqttHeader OriginalRequestHeader { get; set; }
        public required TaskCompletionSource<MqttResponse> TaskSource { get; set; }
    }

    internal List<MqttPendingRequest> PendingRequestList;

    public MqttSingleton(IServiceScopeFactory scopeFactory)
    {
        _requestHandlers = new List<IMqttRequestHandler>();
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
            .WithClientId(clientId)
            .WithTcpServer(brokerAddress, brokerPort) 
            .WithCleanSession()
            .WithKeepAlivePeriod(new TimeSpan(0,5,0))
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += MessageHandler;
    }

    public async Task ConnectAsync()
    {
        if (!_mqttClient.IsConnected)
        {
            await _mqttClient.ConnectAsync(_mqttOptions);

            var stationsConfigsTopic = string.Format(CommunicationConstants.MQTT_CONFIG_TOPIC_FORMAT,
                                            CommunicationConstants.MQTT_SERVER_TARGET);
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(stationsConfigsTopic).Build());
            
            var stationsSensorsTopic = string.Format(CommunicationConstants.MQTT_SENSOR_TOPIC_FORMAT, 
                                                        CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                        CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                        CommunicationConstants.MQTT_SERVER_TARGET);
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(stationsSensorsTopic).Build());

            var stationsActuatorsTopic = string.Format(CommunicationConstants.MQTT_ACTUATOR_TOPIC_FORMAT, 
                                                        CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                        CommunicationConstants.MQTT_SERVER_TARGET);
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(stationsActuatorsTopic).Build());
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
                RequestIdentifier = request.Header.RequestIdentifier,
                DateTime = DateTime.Now
            };
        }
        else
        {
            errorHeader = new MqttHeader()
            {
                RequestEmitter = DefaultIdentifiers.DEFAULT_SENDER_ID,
                RequestIdentifier = DefaultIdentifiers.DEFAULT_REQUEST_ID,
                DateTime = DateTime.Now
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
            RequestIdentifier = request.Header.RequestIdentifier,
            DateTime = DateTime.Now
        };

        var successResponse = MqttResponse.Success(successHeader, (int)objectType, data);
        var responseTopic = originTopic.Replace(CommunicationConstants.MQTT_SERVER_TARGET,
                                                CommunicationConstants.MQTT_STATION_TARGET);

        await PublishAsync(responseTopic, JsonSerializer.Serialize(successResponse));
    }

    public async Task SendRequest(MqttHeader requestHeader, string targetTopic, ObjectTypes objectType, Object data)
    {
        string? rawData = data.ToString();
        string jsonObject = rawData != null ? JsonSerializer.Serialize(rawData) : string.Empty;

        var serverRequest = new MqttRequest(requestHeader,
                                            jsonObject, 
                                            jsonObject.Length,
                                            (int)objectType);

        await PublishAsync(targetTopic, JsonSerializer.Serialize(serverRequest));

        return;
    }

    public void RegisterHandler(IMqttRequestHandler handler)
    {
        _requestHandlers.Add(handler);
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

    private async Task PublishAsync(string topic, string payload)
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
        bool handled = false;
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
                    handled = true;
                    handler.Handle(request, e.ApplicationMessage.Topic);
                    break;
                }
            }
        }

        if (request == null && response == null)
        {
            await SendErrorResponse(null,
                                    e.ApplicationMessage.Topic,
                                    "Unable to deserialize Mqtt request nor Mqtt response",
                                    Status.PARSE_ERROR);
        }

        if (!handled)
        {
            await SendErrorResponse(request,
                                    e.ApplicationMessage.Topic, 
                                    "Server is not able to handle your request", 
                                    Status.CONTRACT_ERROR);
        }

        return;
    }
}