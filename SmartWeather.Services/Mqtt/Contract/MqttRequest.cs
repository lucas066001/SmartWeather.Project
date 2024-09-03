namespace SmartWeather.Services.Mqtt.Contract;

public class MqttRequest
{
    public MqttHeader Header { get; set; } = null!;
    public required string JsonObject { get; set; }
    public int JsonLenght { get; set; }
    public ObjectTypes JsonType { get; set; }
}
