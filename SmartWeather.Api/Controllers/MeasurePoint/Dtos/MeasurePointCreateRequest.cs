using SmartWeather.Entities.MeasurePoint;

namespace SmartWeather.Api.Controllers.MeasurePoint.Dtos;

public class MeasurePointCreateRequest
{
    public int LocalId { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int Unit { get; set; }
    public int ComponentId { get; set; }
}
