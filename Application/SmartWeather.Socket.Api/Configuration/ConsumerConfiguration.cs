using SmartWeather.Socket.Api.Kafka;

namespace SmartWeather.Socket.Api.Configuration;

public static class ConsumerConfiguration
{
    public static void AddConsumer(this IServiceCollection services)
    {
        services.AddHostedService<MeasurePointConsumer>();
    }
}
