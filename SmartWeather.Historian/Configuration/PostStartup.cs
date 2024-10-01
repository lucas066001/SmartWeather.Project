using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Mqtt.Handlers;
using SmartWeather.Services.Stations;

namespace SmartWeather.Historian.Configuration;

public class PostStartup : IHostedService
{
    private readonly MqttSingleton _mqttService;
    private readonly StationService _stationService;
    private readonly ComponentService _componentService;
    private readonly MeasureDataService _componentDataService;
    private readonly MeasurePointService _measurePointService;

    public PostStartup(MqttSingleton mqttService, IServiceScopeFactory scopeFactory)
    {
        _mqttService = mqttService;
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();
        _componentService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentService>();
        _componentDataService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasureDataService>();
        _measurePointService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasurePointService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttService.ConnectAsync();
        _mqttService.RegisterRequestHandler(new ConfigRequestHandler(_mqttService, _stationService, _componentService, _measurePointService));
        _mqttService.RegisterMessageHandler(new MeasureDataMessageHandler(_mqttService, _componentDataService));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}