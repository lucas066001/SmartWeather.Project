namespace SmartWeather.Services.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using SmartWeather.Services.Mqtt;
using System;
using System.Threading;

public class KafkaConsumerSingleton
{
    private IConsumer<string, string> _consumer;
    private bool _isRunning = false;

    private string _bootstrapServers;
    private string _groupId;
    private List<string> _topics;
    private List<IKafkaMessageHandler> _messageHandlers;

    public KafkaConsumerSingleton(IConfiguration configuration)
    {
        _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? throw new Exception("Unable to retreive config : Kafka:BootstrapServers");
        _groupId = configuration["Kafka:GroupId"] ?? throw new Exception("Unable to retreive config : Kafka:GroupId");
        _topics = configuration.GetSection("Kafka:Topics").Get<List<string>>() ?? throw new Exception("Unable to retreive config : Kafka:Topics");
        _messageHandlers = new List<IKafkaMessageHandler>();

        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,  // Auto-commit of offsets
            SessionTimeoutMs = 10000, // Timeout for connection (can increase if network is slow)
            HeartbeatIntervalMs = 3000 // Interval between heartbeats to the broker
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(_topics);
    }

    public void RegisterMessageHandler(IKafkaMessageHandler handler)
    {
        _messageHandlers.Add(handler);
    }

    public void StartConsuming(CancellationToken cancellationToken)
    {
        if (_isRunning)
        {
            Console.WriteLine("KafkaConsumerSingleton is already running.");
            return;
        }

        _isRunning = true;

        try
        {
            Console.WriteLine("KafkaConsumerSingleton is consuming messages...");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    //Console.WriteLine($"Message received: {consumeResult.Message.Value}");
                    foreach (var handler in _messageHandlers)
                    {
                        if (handler.IsAbleToHandle(consumeResult.Topic))
                        {
                            handler.Handle(consumeResult);
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error while consuming message: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Kafka consumption was canceled.");
        }
        finally
        {
            _consumer.Close();
            _isRunning = false;
        }
    }
}
