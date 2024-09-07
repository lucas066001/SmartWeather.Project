namespace SmartWeather.Services.Mqtt;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using SmartWeather.Entities.Station;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Options;
using SmartWeather.Services.Stations;

public class MqttService
{
    private readonly IMqttClient mqttClient;
    private readonly MqttClientOptions mqttOptions;
    private List<IMqttHandler> _requestHandlers;
    private StationService _stationService;

    public MqttService(IServiceScopeFactory scopeFactory)
    {
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();
        _requestHandlers = new List<IMqttHandler>();

        IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

        // Create a new MQTT client
        mqttClient = new MqttFactory().CreateMqttClient();

        var clientId = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.ClientId)];
        var brokerAddress = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.BrokerAddress)];
        int.TryParse(configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.BrokerPort)], out int brokerPort);
        var username = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.Username)];
        var password = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.Password)];

        mqttOptions = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(brokerAddress, brokerPort) 
            .WithCleanSession()
            .WithKeepAlivePeriod(new TimeSpan(0,5,0))
            .Build();

        mqttClient.ApplicationMessageReceivedAsync += MessageHandler;
    }

    public async Task ConnectAsync()
    {
        if (!mqttClient.IsConnected)
        {
            await mqttClient.ConnectAsync(mqttOptions);

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(CommunicationConstants.MQTT_CONFIG_REQUEST_TOPIC).Build());

            IEnumerable<Station> allStations = _stationService.GetAll();

            foreach (var station in allStations)
            {
                var stationTopic = string.Format(CommunicationConstants.MQTT_COMPONENT_DATA_TOPIC_FORMAT, station.Id.ToString());
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(stationTopic).Build());
            }
        }
    }

    public async Task DisconnectAsync()
    {
        if (mqttClient.IsConnected)
        {
            await mqttClient.DisconnectAsync();
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

        var errorResponse = MqttResponse.Failure(errorHeader, string.Format(BaseResponses.FORMAT_ERROR, errorMessage), status);
        await PublishAsync(originTopic.Replace("request", "response"), JsonSerializer.Serialize(errorResponse));
    }

    public async Task SendSuccessResponse(MqttRequest request, string originTopic, ObjectTypes objectType, Object data)
    {
        var successHeader = new MqttHeader()
        {
            RequestEmitter = request.Header.RequestEmitter,
            RequestIdentifier = request.Header.RequestIdentifier,
            DateTime = DateTime.Now
        };

        var errorResponse = MqttResponse.Success(successHeader, objectType, data);
        await PublishAsync(originTopic.Replace("request", "response"), JsonSerializer.Serialize(errorResponse));
    }

    public void RegisterHandler(IMqttHandler handler)
    {
        _requestHandlers.Add(handler);
    }
    
    public async Task<T?> RetreiveMqttObject<T>(string message, string topic, MqttRequest? request = null) where T : class
    {
        T? objectFound = null;

        try
        {
            objectFound = JsonSerializer.Deserialize<T>(message);
        }
        catch (Exception ex)
        {
            await SendErrorResponse(request, topic, "Unable to deserialize Mqtt object : " + ex.Message, Status.PARSE_ERROR);
        }

        return objectFound;
    }

    private async Task PublishAsync(string topic, string payload)
    {
        if (!mqttClient.IsConnected)
        {
            throw new InvalidOperationException("MQTT client is not connected.");
        }

        var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag(false)
            .Build();

        await mqttClient.PublishAsync(mqttMessage);
    }

    private async Task MessageHandler(MqttApplicationMessageReceivedEventArgs e)
    {
        bool handled = false;
        var request = await RetreiveMqttObject<MqttRequest>(e.ApplicationMessage.ConvertPayloadToString(), e.ApplicationMessage.Topic);

        if (request == null)
        {
            await SendErrorResponse(request, e.ApplicationMessage.Topic);
            return;
        }

        foreach (var handler in _requestHandlers)
        {
            if (handler.IsAbleToHandle(request.JsonType))
            {
                handled = true;
                handler.Handle(request, e.ApplicationMessage.Topic);
                break;
            }
        }

        if (!handled)
        {
            await SendErrorResponse(request, e.ApplicationMessage.Topic, "Server is not able to handle your request", Status.CONTRACT_ERROR);
        }

        return;
    }
}