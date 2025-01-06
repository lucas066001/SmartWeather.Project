#ifndef MQTT_CONTRACT_CONSTANTS_H
#define MQTT_CONTRACT_CONSTANTS_H

#include <Arduino.h>

namespace SmartWeather::Constants::Mqtt
{
    enum class JsonMessageType
    {
        UNKNOWN,
        CONFIG_REQUEST,
        CONFIG_RESPONSE,
        ACTUATOR_REQUEST,
        ACTUATOR_RESPONSE
    };

    enum class ExecutionResults
    {
        INTERNAL_ERROR,
        TIMEOUT_ERROR,
        DATABASE_ERROR,
        PARSE_ERROR,
        CONTRACT_ERROR,
        OK
    };

    class MqttBaseResponses
    {
    public:
        static const char *OK;
        static const char *ARGUMENT_ERROR;
        static const char *INTERNAL_ERROR;
        static const char *TIMEOUT_ERROR;
        static const char *CONTRACT_ERROR;
        static const char *DATABASE_ERROR;
        static const char *FORMAT_ERROR;
    };

    class DefaultIdentifiers
    {
    public:
        static const char *DEFAULT_REQUEST_ID;
        static const char *DEFAULT_SENDER_ID;
    };
}

#endif