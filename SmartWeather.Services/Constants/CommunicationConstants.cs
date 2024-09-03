namespace SmartWeather.Services.Constants;

public class CommunicationConstants
{
    public const string MQTT_CONFIG_TOPIC = "cb57dbf9-16d3-4f43-bc5d-f539621c3be1/config";
    public const string MQTT_COMPONENT_DATA_TOPIC = "ComponentData";
    public const string MQTT_COMPONENT_ACTUATOR_TOPIC = "Actuator";
    public const string MQTT_TOPIC_FORMAT = "{0}/Station/{1}/{2}";
}
