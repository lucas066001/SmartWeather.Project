using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.Kafka;
using SmartWeather.Services.Kafka.Handlers;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Stations;
using SmartWeather.Socket.Api.Hubs.MeasurePoint;
using SmartWeather.Socket.Api.Kafka.Handlers;

namespace SmartWeather.Socket.Api.Kafka;

public class MeasurePointConsumer : IHostedService
{
    private readonly KafkaConsumerSingleton _kafkaConsumer;
    //private readonly MeasurePointHub _measurePointHub;
    private readonly CancellationTokenSource _serviceCancel;
    private readonly IHubContext<MeasurePointHub> _measurePointHub;

    public MeasurePointConsumer(KafkaConsumerSingleton kafkaConsumer, IHubContext<MeasurePointHub> measurePointHub, IServiceScopeFactory scopeFactory)
    {
        _measurePointHub = measurePointHub;
        _serviceCancel = new CancellationTokenSource();
        _kafkaConsumer = kafkaConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _kafkaConsumer.RegisterMessageHandler(new MeasureDataStreamHandler(_measurePointHub));
        Task.Run(() => _kafkaConsumer.StartConsuming(_serviceCancel.Token));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _serviceCancel.Cancel();
        return Task.CompletedTask;
    }
}