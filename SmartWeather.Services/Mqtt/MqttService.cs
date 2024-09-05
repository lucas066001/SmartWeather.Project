namespace SmartWeather.Services.Mqtt;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Services.Components;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.Services.Mqtt.Dtos.Converters;
using SmartWeather.Services.Options;
using SmartWeather.Services.Stations;

public class MqttService
{
    private readonly IMqttClient mqttClient;
    private readonly MqttClientOptions mqttOptions;
    private readonly StationService _stationService;
    private readonly ComponentService _componentService;

    public MqttService(IServiceScopeFactory scopeFactory)
    {
        _componentService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentService>();
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();

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
            .Build();

        mqttClient.ApplicationMessageReceivedAsync += MessageHandler;
    }

    public async Task ConnectAsync()
    {
        if (!mqttClient.IsConnected)
        {
            await mqttClient.ConnectAsync(mqttOptions);

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(CommunicationConstants.MQTT_CONFIG_TOPIC).Build());
            
            // Will need to add subscribtion to all station topics
            // retreive all station, go accross and subscribe
        }
    }

    public async Task DisconnectAsync()
    {
        if (mqttClient.IsConnected)
        {
            await mqttClient.DisconnectAsync();
        }
    }

    public async Task PublishAsync(string topic, string payload)
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

    public Task MessageHandler(MqttApplicationMessageReceivedEventArgs e)
    {
        if(e.ApplicationMessage.Topic == CommunicationConstants.MQTT_CONFIG_TOPIC)
        {
            // Should be a registering request
            Task.Run(() => HandleConfigRequest(e.ApplicationMessage));
        }
        else if (e.ApplicationMessage.Topic.Contains(CommunicationConstants.MQTT_COMPONENT_DATA_TOPIC))
        {
            // Should be a saving data action
            Task.Run(() => HandleSavingRequest(e.ApplicationMessage));
        }
        else if (e.ApplicationMessage.Topic.Contains(CommunicationConstants.MQTT_COMPONENT_ACTUATOR_TOPIC))
        {
            // Should be a response to actuator command
            Task.Run(() => HandleActuatorResponse(e.ApplicationMessage));
        }

        return Task.CompletedTask;
    }

    private async Task<T?> RetreiveMqttObject<T>(string message, string topic, MqttHeader? errorHeader = null) where T : class
    {
        T? objectFound = null;

        if (errorHeader == null)
        {
            errorHeader = new MqttHeader()
            {
                RequestEmitter = DefaultIdentifiers.DEFAULT_SENDER_ID,
                RequestIdentifier = DefaultIdentifiers.DEFAULT_REQUEST_ID,
                DateTime = DateTime.Now,
            };
        }

        try
        {
            objectFound = JsonSerializer.Deserialize<T>(message);
        }
        catch (Exception ex)
        {
            var errorResponse = MqttResponse.Failure(errorHeader, string.Format(BaseResponses.FORMAT_ERROR, "Unable to parse Mqtt Object -> " + ex.Message));
            await PublishAsync(topic, JsonSerializer.Serialize(errorResponse));
        }

        return objectFound;
    }

    public async Task SendErrorResponse(MqttApplicationMessage message, string errorMessage = BaseResponses.INTERNAL_ERROR)
    {
        var errorHeader = new MqttHeader()
        {
            RequestEmitter = DefaultIdentifiers.DEFAULT_SENDER_ID,
            RequestIdentifier = DefaultIdentifiers.DEFAULT_REQUEST_ID,
            DateTime = DateTime.Now,
        };

        var errorResponse = MqttResponse.Failure(errorHeader, string.Format(BaseResponses.FORMAT_ERROR, errorMessage));
        await PublishAsync(message.Topic, JsonSerializer.Serialize(errorResponse));
    }

    public async Task HandleConfigRequest(MqttApplicationMessage message)
    {
        var request = await RetreiveMqttObject<MqttRequest>(message.ConvertPayloadToString(), message.Topic);

        if (request == null)
        {
            await SendErrorResponse(message, "Unable to cast your request");
            return;
        }

        if (!Enum.IsDefined(typeof(ObjectTypes), request.JsonType) || request.JsonType != (int)ObjectTypes.CONFIG_REQUEST)
        {
            await SendErrorResponse(message, "Unknown object type -> " + request.JsonType.ToString());
            return;
        }

        var configRequest = await RetreiveMqttObject<StationConfigRequest>(request.JsonObject, message.Topic, request.Header);

        if (configRequest == null)
        {
            await SendErrorResponse(message);
            return;
        }

        var macAdress = configRequest.MacAddress;

        Station? retrievedStation = _stationService.GetStationByMacAddress(macAdress);

        if (retrievedStation != null)
        {
            retrievedStation.Components = _componentService.GetFromStation(retrievedStation.Id).ToList();
            var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(retrievedStation);
            MqttResponse response = MqttResponse.Success(request.Header, ObjectTypes.CONFIG_RESPONSE, formattedData);
            await PublishAsync(message.Topic, JsonSerializer.Serialize(response));
        }
        else
        {
            Station newStation;
            try
            {
                newStation = _stationService.AddGenericStation(macAdress);
            }
            catch (Exception ex)
            {
                await SendErrorResponse(message, "Unable to create a station : " + ex.Message);
                return;
            }

            if (configRequest.ActivePins.Any())
            {
                try
                {
                    newStation.Components = _componentService.AddGenericComponentPool(newStation.Id, configRequest.ActivePins).ToList();
                }
                catch (Exception ex)
                {
                    // Undo station creation and send error details
                    _stationService.DeleteStation(newStation.Id);
                    await SendErrorResponse(message, "Unable to create a station : " + ex.Message);
                    return;
                }

                var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(newStation);
                MqttResponse response = MqttResponse.Success(request.Header, ObjectTypes.CONFIG_RESPONSE, formattedData);
                await PublishAsync(message.Topic, JsonSerializer.Serialize(response));
            }

        }
    }

    public Task HandleSavingRequest(MqttApplicationMessage message)
    {

        return Task.CompletedTask;
    }

    public Task HandleActuatorResponse(MqttApplicationMessage message)
    {

        return Task.CompletedTask;
    }

}