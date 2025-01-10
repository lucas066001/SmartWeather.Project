namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ActuatorCommandRequest
{
    public int StationId { get; set; }
    public int ComponentGpioPin { get; set; }
    public int ComponentValueUpdate { get; set; }
}
