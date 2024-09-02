namespace SmartWeather.Api.Controllers.ComponentData.Dtos;

public class ComponentDataUpdateRequest
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public int Value { get; set; }
    public DateTime DateTime { get; set; }
}
