namespace SmartWeather.Services.Mqtt;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Options;

public class MqttService
{
    private static readonly Lazy<MqttService> instance = new Lazy<MqttService>(() => new MqttService());
    private readonly IMqttClient mqttClient;
    private readonly MqttClientOptions mqttOptions;
    public static MqttService Instance => instance.Value;

    private MqttService()
    {
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
            .WithCredentials(username, password) 
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

    public async Task HandleConfigRequest(MqttApplicationMessage message)
    {
        // Parse JSON object
        // Retreive necessary infos to build :
        // ->Station
        // ->Components

        MqttRequest request;

        try
        {
            request = JsonSerializer.Deserialize<MqttRequest>(message.ConvertPayloadToString());
        }
        catch (Exception ex)
        {
            var header = new MqttHeader() {
                RequestEmitter = DefaultIdentifiers.DEFAULT_SENDER_ID,
                RequestIdentifier = DefaultIdentifiers.DEFAULT_REQUEST_ID,
                DateTime = DateTime.Now,
            };

            var errorResponse = MqttResponse.Failure(header, string.Format(BaseResponses.FORMAT_ERROR, "Unable to parse MqttRequest -> " + ex.Message ));
            
            await PublishAsync(message.Topic, JsonSerializer.Serialize(errorResponse));
        }

        //StationConfigRequest

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