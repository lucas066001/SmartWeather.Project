using MQTTnet;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.Constants;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Mqtt.Handlers;
using SmartWeather.Services.Stations;

namespace SmartWeather.Historian.Configuration;

public class PostStartup : IHostedService
{
    private readonly MqttSingleton _mqttSingleton;
    private readonly StationService _stationService;
    private readonly ComponentService _componentService;
    private readonly MeasureDataService _componentDataService;
    private readonly MeasurePointService _measurePointService;

    public PostStartup(MqttSingleton mqttSingleton, IServiceScopeFactory scopeFactory)
    {
        _mqttSingleton = mqttSingleton;
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();
        _componentService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentService>();
        _componentDataService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasureDataService>();
        _measurePointService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasurePointService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttSingleton.ConnectAsync();

        var stationsSensorsTopic = string.Format(CommunicationConstants.MQTT_SENSOR_TOPIC_FORMAT,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SERVER_TARGET);
        await _mqttSingleton.SubscribeAsync(stationsSensorsTopic);

        _mqttSingleton.RegisterMessageHandler(new MeasureDataMessageHandler(_mqttSingleton, _componentDataService));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}