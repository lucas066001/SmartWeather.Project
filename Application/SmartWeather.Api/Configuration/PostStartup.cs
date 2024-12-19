using MQTTnet;
using SmartWeather.Api.Mqtt.Handlers;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.Constants;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Stations;

namespace SmartWeather.Api.Configuration;

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

        var stationsConfigsTopic = string.Format(CommunicationConstants.MQTT_CONFIG_TOPIC_FORMAT,
                                CommunicationConstants.MQTT_SERVER_TARGET);
        await _mqttSingleton.SubscribeAsync(stationsConfigsTopic);

        var stationsActuatorsTopic = string.Format(CommunicationConstants.MQTT_ACTUATOR_TOPIC_FORMAT,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SINGLE_LEVEL_WILDCARD,
                                                    CommunicationConstants.MQTT_SERVER_TARGET);
        await _mqttSingleton.SubscribeAsync(stationsActuatorsTopic);

        _mqttSingleton.RegisterRequestHandler(new ConfigRequestHandler(_mqttSingleton, _stationService, _componentService, _measurePointService));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}