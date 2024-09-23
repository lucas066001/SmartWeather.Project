using SmartWeather.Entities.Component;

namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ComponentCreateRequest
{
    public required string Name { get; set; }
    public required string Color { get; set; }
    public int GpioPin { get; set; }
    public int Type { get; set; }
    public int StationId { get; set; }
}
