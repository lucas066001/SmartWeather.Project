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

    public PostStartup(MqttService mqttService, IServiceScopeFactory scopeFactory)
    {
        _mqttService = mqttService;
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();
        _componentService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentService>();
    }

    // you may wish to make use of the cancellationToken
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttService.ConnectAsync();
        _mqttService.RegisterHandler(new ConfigRequestHandler(_mqttService, _stationService, _componentService));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}