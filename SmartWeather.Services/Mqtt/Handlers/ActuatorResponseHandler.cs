using MQTTnet;
using SmartWeather.Services.Mqtt.Contract;

namespace SmartWeather.Services.Mqtt.Handlers;

public class ActuatorResponseHandler : IMqttHandler
{
    public void Handle(MqttRequest message, string originTopic)
    {
        throw new NotImplementedException();
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType)
                && (ObjectTypes)requestType == ObjectTypes.ACTUATOR_RESPONSE;
    }
}
