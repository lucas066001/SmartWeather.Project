namespace SmartWeather.Services.Kafka;

using Confluent.Kafka;

public interface IKafkaMessageHandler
{
    public void Handle(ConsumeResult<string, string> message);
    public bool IsAbleToHandle(string kafkaTopic);
}
