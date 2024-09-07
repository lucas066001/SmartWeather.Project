using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Mqtt.Handlers;
using SmartWeather.Services.Stations;

namespace SmartWeather.Api.Configuration;

public class PostStartup : IHostedService
{
    private readonly MqttService _mqttService;
    private readonly StationService _stationService;
    private readonly ComponentService _componentService;
    private readonly ComponentDataService _componentDataService;

    public PostStartup(MqttService mqttService, IServiceScopeFactory scopeFactory)
    {
        _mqttService = mqttService;
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();
        _componentService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentService>();
        _componentDataService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentDataService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttService.ConnectAsync();
        _mqttService.RegisterHandler(new ConfigRequestHandler(_mqttService, _stationService, _componentService));
        _mqttService.RegisterHandler(new SavingRequestHandler(_mqttService, _componentDataService));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}