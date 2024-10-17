namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ActuatorCommandRequest
{
    public int ComponentId { get; set; }
    public int StationId { get; set; }
    public int ComponentValueUpdate { get; set; }
}
