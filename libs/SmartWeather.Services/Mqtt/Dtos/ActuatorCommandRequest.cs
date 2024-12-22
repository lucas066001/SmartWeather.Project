using SmartWeather.Entities.Station;

namespace SmartWeather.Services.Mqtt.Dtos;

public class ActuatorCommandRequest
{
    public int GpioPin { get; set; }
    public int Value { get; set; }
}
