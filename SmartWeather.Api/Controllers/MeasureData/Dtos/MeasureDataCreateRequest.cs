namespace SmartWeather.Api.Controllers.ComponentData.Dtos;

public class MeasureDataCreateRequest
{
    public int MeasurePointId { get; set; }
    public int Value { get; set; }
    public DateTime DateTime { get; set; }
}
