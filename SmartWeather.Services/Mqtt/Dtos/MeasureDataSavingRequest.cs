namespace SmartWeather.Services.Mqtt.Dtos;

public class MeasureDataSavingRequest
{
    public int MeasurePointId { get; set; }
    public float Value { get; set; }
}
