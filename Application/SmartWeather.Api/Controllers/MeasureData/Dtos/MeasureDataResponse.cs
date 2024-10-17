namespace SmartWeather.Api.Controllers.ComponentData.Dtos;

public class MeasureDataResponse
{
    public int MeasurePointId { get; set; }
    public float Value { get; set; }
    public DateTime DateTime { get; set; }
}
