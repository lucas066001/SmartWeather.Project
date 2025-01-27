using Microsoft.AspNetCore.SignalR;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt;
using SmartWeather.Socket.Api.Hubs.MeasurePoint;
using SmartWeather.Socket.Api.Mqtt.Handlers;

namespace SmartWeather.Socket.Api.HostedServices;

public class MeasurePointConsumer : IHostedService
{
    private readonly MqttSingleton _mqttSingleton;
    private readonly CancellationTokenSource _serviceCancel;
    private readonly IHubContext<MeasurePointHub> _measurePointHub;

    public MeasurePointConsumer(MqttSingleton mqttSingleton, IHubContext<MeasurePointHub> measurePointHub, IServiceScopeFactory scopeFactory)
    {
        _measurePointHub = measurePointHub;
        _serviceCancel = new CancellationTokenSource();
        _mqttSingleton = mqttSingleton;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttSingleton.ConnectAsync();

        var measurePointsTopic = string.Format(CommunicationConstants.MQTT_SENSOR_TOPIC_FORMAT,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SERVER_TARGET);

        await _mqttSingleton.SubscribeAsync(measurePointsTopic);

        _mqttSingleton.RegisterMessageHandler(new MeasureDataStreamHandler(_measurePointHub));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _serviceCancel.Cancel();
        return Task.CompletedTask;
    }
}