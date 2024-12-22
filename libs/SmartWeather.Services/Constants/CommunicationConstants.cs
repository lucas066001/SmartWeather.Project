namespace SmartWeather.Services.Constants;

public class CommunicationConstants
{
    public const string MQTT_CONFIG_TOPIC_FORMAT = "cb57dbf9-16d3-4f43-bc5d-f539621c3be1/config/{0}";
    public const string MQTT_SENSOR_TOPIC_FORMAT = "station/{0}/measure_point/{1}/{2}";
    public const string MQTT_ACTUATOR_TOPIC_FORMAT = "station/{0}/actuators/{1}";
    public const string MQTT_SINGLE_LEVEL_WILDCARD = "+";
    public const string MQTT_STATION_TARGET = "tostation";
    public const string MQTT_SERVER_TARGET = "toserver";
}
