using SmartWeather.Entities.Station;

namespace SmartWeather.Services.Mqtt.Dtos;

public class ActuatorCommandRequest
{
    public int ComponentId { get; set; }
    public int NewValue { get; set; }
}
