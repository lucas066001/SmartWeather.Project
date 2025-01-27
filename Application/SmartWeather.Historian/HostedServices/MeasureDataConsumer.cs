using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Kafka.Handlers;
using SmartWeather.Services.Mqtt;

namespace SmartWeather.Historian.Configuration;

public class MeasureDataConsumer : IHostedService
{
    private readonly MqttSingleton _mqttSingleton;
    private readonly MeasureDataService _measureDataService;
    private readonly CancellationTokenSource _serviceCancel;

    public MeasureDataConsumer(MqttSingleton mqttSingleton, IServiceScopeFactory scopeFactory)
    {
        _serviceCancel= new CancellationTokenSource();
        _mqttSingleton = mqttSingleton;
        _measureDataService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasureDataService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttSingleton.ConnectAsync();

        var measurePointsTopic = string.Format(CommunicationConstants.MQTT_SENSOR_TOPIC_FORMAT,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SERVER_TARGET);

        await _mqttSingleton.SubscribeAsync(measurePointsTopic);

        _mqttSingleton.RegisterMessageHandler(new MeasureDataMessageHandler(_measureDataService));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _serviceCancel.Cancel();
        return Task.CompletedTask;
    }
}