namespace SmartWeather.Services.Mqtt.Contract;

public class MqttHeader
{
    public required string RequestEmitter { get; set; }
    public required string RequestIdentifier { get; set; }
    public DateTime DateTime { get; set; }

}
