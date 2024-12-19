using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Kafka;
using SmartWeather.Services.Kafka.Handlers;

namespace SmartWeather.Historian.Configuration;

public class PostStartup : IHostedService
{
    private readonly KafkaConsumerSingleton _kafkaConsumer;
    private readonly MeasureDataService _measureDataService;
    private readonly CancellationTokenSource _serviceCancel;

    public PostStartup(KafkaConsumerSingleton kafkaConsumer, IServiceScopeFactory scopeFactory)
    {
        _serviceCancel= new CancellationTokenSource();
        _kafkaConsumer = kafkaConsumer;
        _measureDataService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MeasureDataService>();
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