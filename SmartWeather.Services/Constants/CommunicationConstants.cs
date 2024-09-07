namespace SmartWeather.Services.Constants;

public class CommunicationConstants
{
    public const string MQTT_CONFIG_REQUEST_TOPIC = "cb57dbf9-16d3-4f43-bc5d-f539621c3be1/config/request";
    public const string MQTT_CONFIG_RESPONSE_TOPIC = "cb57dbf9-16d3-4f43-bc5d-f539621c3be1/config/response";
    public const string MQTT_COMPONENT_DATA_TOPIC_FORMAT = "station/{1}/sensors";
    public const string MQTT_COMPONENT_ACTUATOR_REQUEST_TOPIC_FORMAT = "station/{1}/actuators/request";
    public const string MQTT_COMPONENT_ACTUATOR_RESPONSE_TOPIC_FORMAT = "station/{1}/actuators/response";
}
