namespace SmartWeather.Services.Mqtt.Dtos;

public class ComponentDataSavingRequest
{
    public int ComponentId { get; set; }
    public float Value { get; set; }
    public DateTime DateTime { get; set; }
}
