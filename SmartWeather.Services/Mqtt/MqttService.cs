using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos;

namespace SmartWeather.Services.Mqtt;

public class MqttService(MqttSingleton mqttSingleton)
{
    public bool SendActuatorCommand(int stationId, int componenetId, int newValue)
    {
        var result = false;
        var request = new ActuatorCommandRequest(componenetId, newValue);
        var targetTopic = String.Format(CommunicationConstants.MQTT_ACTUATOR_TOPIC_FORMAT,
                                        stationId.ToString(),
                                        CommunicationConstants.MQTT_SERVER_TARGET);

        //var result = mqttSingleton.SendRequest(targetTopic,
        //                                        ObjectTypes.ACTUATOR_REQUEST,
        //                                        request);

        return result;
    }
}
