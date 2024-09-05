using SmartWeather.Services.Mqtt;

namespace SmartWeather.Api.Configuration;

public class PostStartup : IHostedService
{
    private readonly MqttService _mqttService;

    public PostStartup(MqttService mqttService)
    {
        _mqttService = mqttService;
    }

    // you may wish to make use of the cancellationToken
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttService.ConnectAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}