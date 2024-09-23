namespace SmartWeather.Api.Controllers.ComponentData.Dtos;

public class MeasureDataUpdateRequest
{
    public int Id { get; set; }
    public int MeasurePointId { get; set; }
    public int Value { get; set; }
    public DateTime DateTime { get; set; }
}
