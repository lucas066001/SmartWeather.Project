namespace SmartWeather.Services.Options;

public class Mqtt
{
    public string ClientId { get; init; } = null!;
    public string EmitterId { get; init; } = null!;
    public string BrokerAddress { get; init; } = null!;
    public int BrokerPort { get; init; }
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
