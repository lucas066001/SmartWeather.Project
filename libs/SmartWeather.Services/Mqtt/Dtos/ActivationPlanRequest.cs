using SmartWeather.Entities.Station;

namespace SmartWeather.Services.Mqtt.Dtos;

public class ActivationPlanRequest
{
    public IEnumerable<int> ActuatorIds { get; set; } = new List<int>();
}
