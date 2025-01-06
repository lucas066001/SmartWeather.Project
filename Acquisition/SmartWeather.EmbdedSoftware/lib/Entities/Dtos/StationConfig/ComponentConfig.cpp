#include <Dtos/StationConfig/ComponentConfig.h>

using namespace SmartWeather::Entities::Dtos;

void ComponentConfig::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        GpioPin = json["GpioPin"].as<int>();
        DatabaseId = json["DatabaseId"].as<int>();

        JsonArray array = json["MeasurePointsConfigs"].as<JsonArray>();
        for (JsonVariant v : array)
        {
            MeasurePointConfigResponse mpConfigRetreived;
            mpConfigRetreived.FromString(v.as<String>());
            MeasurePointsConfigs.push_back(mpConfigRetreived);
        }

        if (DatabaseId == 0)
        {
            throw std::runtime_error("Invalid deserialization");
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String ComponentConfig::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["GpioPin"] = GpioPin;
    json["DatabaseId"] = DatabaseId;

    JsonArray array = json["MeasurePointsConfigs"].to<JsonArray>();

    for (MeasurePointConfigResponse mpConfigRetreived : MeasurePointsConfigs)
    {
        JsonObject arrayItem = array.add<JsonObject>();
        mpConfigRetreived.ToJson(arrayItem);
    }

    String output;
    serializeJson(doc, output);
    return output;
}

void ComponentConfig::ToJson(JsonObject &json)
{
    json["GpioPin"] = GpioPin;
    json["DatabaseId"] = DatabaseId;
}

void ComponentConfig::FromJson(JsonObject &json)
{
    GpioPin = json["GpioPin"].as<int>();
    DatabaseId = json["DatabaseId"].as<int>();
}
