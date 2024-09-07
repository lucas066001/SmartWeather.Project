namespace SmartWeather.Services.Mqtt.Dtos;

public class ComponentDataSavingRequest
{
    public int ComponentId { get; set; }
    public int Value { get; set; }
    public DateTime DateTime { get; set; }
}
