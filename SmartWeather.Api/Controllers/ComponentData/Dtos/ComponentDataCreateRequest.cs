namespace SmartWeather.Api.Controllers.ComponentData.Dtos;

public class ComponentDataCreateRequest
{
    public int ComponentId { get; set; }
    public int Value { get; set; }
    public DateTime DateTime { get; set; }
}
