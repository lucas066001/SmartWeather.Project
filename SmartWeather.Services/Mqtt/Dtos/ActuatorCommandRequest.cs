using SmartWeather.Entities.Station;

namespace SmartWeather.Services.Mqtt.Dtos;

public class ActuatorCommandRequest
{
    public int ComponentId { get; set; }
    public int NewValue { get; set; }

    public ActuatorCommandRequest(int componentId, int newValue) {
        if (!(componentId > 0))
        {
            throw new Exception("Invalid component Id");
        }

        ComponentId = componentId;
        NewValue = newValue;
    }
}
