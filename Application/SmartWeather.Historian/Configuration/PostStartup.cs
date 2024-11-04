using MQTTnet;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Kafka;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Kafka.Handlers;
using SmartWeather.Services.Stations;

namespace SmartWeather.Historian.Configuration;

public class PostStartup : IHostedService
{
    private readonly KafkaConsumerSingleton _kafkaConsumer;
    private readonly StationService _stationService;
    private readonly ComponentService _componentService;
    private readonly MeasureDataService _measureDataService;
    private readonly MeasurePointService _measurePointService;
    private readonly CancellationTokenSource _serviceCancel;

    public PostStartup(KafkaConsumerSingleton kafkaConsumer, IServiceScopeFactory scopeFactory)
    {
        _serviceCancel= new CancellationTokenSource();
        _kafkaConsumer = kafkaConsumer;
        _stationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<StationService>();
        _componentService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ComponentService>();
        _measureDataService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasureDataService>();
        _measurePointService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasurePointService>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _kafkaConsumer.RegisterMessageHandler(new MeasureDataMessageHandler(_measureDataService));
        _kafkaConsumer.StartConsuming(_serviceCancel.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _serviceCancel.Cancel();
        return Task.CompletedTask;
    }
}