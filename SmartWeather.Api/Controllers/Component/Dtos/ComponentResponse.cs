using SmartWeather.Entities.Component;

namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ComponentResponse
{
    public int Id { get; set; }
    public int GpioPin { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public ComponentUnit Unit { get; set; }
    public ComponentType Type { get; set; }
    public int StationId { get; set; }
}
