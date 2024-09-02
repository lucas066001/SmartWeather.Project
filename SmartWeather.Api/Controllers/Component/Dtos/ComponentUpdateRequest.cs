using SmartWeather.Entities.Component;

namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ComponentUpdateRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public int Unit { get; set; }
    public int Type { get; set; }
    public int StationId { get; set; }
}
