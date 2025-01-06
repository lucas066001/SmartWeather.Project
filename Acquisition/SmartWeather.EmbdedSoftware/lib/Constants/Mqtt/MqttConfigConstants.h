#ifndef MQTT_CONFIG_CONSTANTS_H
#define MQTT_CONFIG_CONSTANTS_H

#include <Arduino.h>
#include <WiFi.h>

namespace SmartWeather::Constants::Mqtt
{
    extern const char *MQTT_HOST;
    extern const IPAddress MQTT_IP;
    extern const int MQTT_PORT;
    extern const int MQTT_MAX_MESSAGE_SIZE;
    extern const int MQTT_TIMEOUT;
    extern const char *MQTT_CONFIG_TOPIC;
    extern const char *MQTT_SENSOR_TOPIC_FORMAT;
    extern const char *MQTT_SINGLE_LEVEL_WILDCARD;
    extern const char *MQTT_ACTUATOR_TOPIC_FORMAT;
    extern const char *MQTT_STATION_TARGET;
    extern const char *MQTT_SERVER_TARGET;
    extern const char *MQTT_CLIENT_ID;
    extern const char *MQTT_USER;
    extern const char *MQTT_PASSWORD;
}

#endif