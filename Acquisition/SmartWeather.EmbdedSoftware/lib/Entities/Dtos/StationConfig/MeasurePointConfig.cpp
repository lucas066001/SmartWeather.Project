#include <Dtos/StationConfig/MeasurePointConfig.h>

using namespace SmartWeather::Entities::Dtos;

void MeasurePointConfig::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        LocalId = json["LocalId"].as<int>();
        DefaultName = json["DefaultName"].as<String>();
        Unit = json["Unit"].as<int>();
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String MeasurePointConfig::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["LocalId"] = LocalId;
    json["DefaultName"] = DefaultName;
    json["Unit"] = Unit;

    String output;
    serializeJson(doc, output);
    return output;
}

void MeasurePointConfig::ToJson(JsonObject &json)
{
    json["LocalId"] = LocalId;
    json["DefaultName"] = DefaultName;
    json["Unit"] = Unit;
}

void MeasurePointConfig::FromJson(JsonObject &json)
{
    LocalId = json["LocalId"].as<int>();
    DefaultName = json["DefaultName"].as<String>();
    Unit = json["Unit"].as<int>();
}
