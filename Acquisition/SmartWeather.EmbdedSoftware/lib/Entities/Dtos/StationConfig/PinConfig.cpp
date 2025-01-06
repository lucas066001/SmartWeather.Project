#include <Dtos/StationConfig/PinConfig.h>

using namespace SmartWeather::Entities::Dtos;

void PinConfig::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        GpioPin = json["GpioPin"].as<int>();
        ComponentType = json["ComponentType"].as<int>();
        DefaultName = json["DefaultName"].as<String>();

        JsonArray array = json["MeasurePoints"].as<JsonArray>();
        for (JsonVariant val : array)
        {
            MeasurePointConfig measurePointConfigRetreived;
            measurePointConfigRetreived.FromString(val.as<String>());
            MeasurePoints.push_back(measurePointConfigRetreived);
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String PinConfig::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["GpioPin"] = GpioPin;
    json["ComponentType"] = ComponentType;
    json["DefaultName"] = DefaultName;
    JsonArray array = json["MeasurePoints"].to<JsonArray>();

    for (MeasurePointConfig measurePointConf : MeasurePoints)
    {
        JsonObject arrayItem = array.add<JsonObject>();
        measurePointConf.ToJson(arrayItem);
    }

    String output;
    serializeJson(doc, output);
    return output;
}

void PinConfig::ToJson(JsonObject &json)
{
    json["GpioPin"] = GpioPin;
    json["ComponentType"] = ComponentType;
    json["DefaultName"] = DefaultName;

    JsonArray array = json["MeasurePoints"].to<JsonArray>();
    for (MeasurePointConfig measurePointConf : MeasurePoints)
    {
        JsonObject arrayItem = array.add<JsonObject>();
        measurePointConf.ToJson(arrayItem);
    }
}

void PinConfig::FromJson(JsonObject &json)
{
    GpioPin = json["GpioPin"].as<int>();
    ComponentType = json["ComponentType"].as<int>();
    DefaultName = json["DefaultName"].as<String>();

    JsonArray array = json["MeasurePoints"].as<JsonArray>();
    for (JsonVariant val : array)
    {
        MeasurePointConfig measurePointConfigRetreived;
        measurePointConfigRetreived.FromString(val.as<String>());
        MeasurePoints.push_back(measurePointConfigRetreived);
    }
}
