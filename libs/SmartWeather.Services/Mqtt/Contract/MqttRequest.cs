namespace SmartWeather.Services.Mqtt.Contract;

public class MqttRequest
{
    public MqttHeader Header { get; set; } = null!;
    public string JsonObject { get; set; }
    public int JsonLenght { get; set; }
    public int JsonType { get; set; }

    public MqttRequest(MqttHeader header, string jsonObject, int jsonLenght, int jsonType)
    {
        Header = header;
        JsonObject = jsonObject;
        JsonLenght = jsonLenght;
        JsonType = jsonType;
    }
}
