using SmartWeather.Api.Controllers.MeasurePoint.Dtos;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;

namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ComponentResponse
{
    public int Id { get; set; }
    public int GpioPin { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public ComponentType Type { get; set; }
    public int StationId { get; set; }
    public List<MeasurePointResponse>? MeasurePoints { get; set; }
}
