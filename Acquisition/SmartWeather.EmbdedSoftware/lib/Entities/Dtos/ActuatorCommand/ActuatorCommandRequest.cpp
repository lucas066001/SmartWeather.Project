#include <Dtos/ActuatorCommand/ActuatorCommandRequest.h>
#include "ActuatorCommandRequest.h"

using namespace SmartWeather::Entities::Dtos;

void ActuatorCommandRequest::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        GpioPin = json["GpioPin"].as<int>();
        Value = json["Value"].as<int>();
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String ActuatorCommandRequest::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["GpioPin"] = GpioPin;
    json["Value"] = Value;

    String output;
    serializeJson(doc, output);
    return output;
}

void ActuatorCommandRequest::ToJson(JsonObject &json)
{
    json["GpioPin"] = GpioPin;
    json["Value"] = Value;
}

void ActuatorCommandRequest::FromJson(JsonObject &json)
{
    GpioPin = json["GpioPin"].as<int>();
    Value = json["Value"].as<int>();
}
