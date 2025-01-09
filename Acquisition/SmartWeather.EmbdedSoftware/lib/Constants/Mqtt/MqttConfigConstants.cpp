#include <Mqtt/MqttConfigConstants.h>

namespace SmartWeather::Constants::Mqtt
{
    const char *MQTT_HOST = "localhost";
    // const IPAddress MQTT_IP(192, 168, 1, 78);192.168.30.81
    const IPAddress MQTT_IP(192, 168, 30, 81);
    // const IPAddress MQTT_IP(10, 101, 53, 165);
    const int MQTT_PORT = 1883;
    const int MQTT_MAX_MESSAGE_SIZE = 2048;
    const int MQTT_TIMEOUT = 120000;
    const char *MQTT_CONFIG_TOPIC = "cb57dbf9-16d3-4f43-bc5d-f539621c3be1/config";
    const char *MQTT_SENSOR_TOPIC_FORMAT = "station/{0}/measure_point/{1}";
    const char *MQTT_SINGLE_LEVEL_WILDCARD = "+";
    const char *MQTT_ACTUATOR_TOPIC_FORMAT = "station/{0}/actuators";
    const char *MQTT_SERVER_TARGET = "/toserver";
    const char *MQTT_STATION_TARGET = "/tostation";
    const char *MQTT_CLIENT_ID = "SmartWeather.EmbdedClient";
    const char *MQTT_USER = "";
    const char *MQTT_PASSWORD = "";
}