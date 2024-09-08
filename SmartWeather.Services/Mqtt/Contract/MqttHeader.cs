namespace SmartWeather.Services.Mqtt.Contract;

using Microsoft.Extensions.Configuration;
using SmartWeather.Services.Options;

public class MqttHeader
{
    public required string RequestEmitter { get; set; }
    public required string RequestIdentifier { get; set; }
    public DateTime DateTime { get; set; }

    public static MqttHeader Generate()
    {
        IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
        var emitterId = configuration.GetSection(nameof(Mqtt))[nameof(Mqtt.EmitterId)];
        if (emitterId == null)
        {
            throw new Exception("Unable to get server emitter id");
        }

        return new MqttHeader
        {
            RequestEmitter = emitterId,
            RequestIdentifier = Guid.NewGuid().ToString(),
            DateTime = DateTime.UtcNow
        };
    }

}
