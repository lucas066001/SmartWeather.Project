#include <Dtos/StationConfig/StationConfigResponse.h>

using namespace SmartWeather::Entities::Dtos;

void StationConfigResponse::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        StationDatabaseId = json["StationDatabaseId"].as<int>();

        JsonArray array = json["ConfigComponents"].as<JsonArray>();
        for (JsonVariant val : array)
        {
            ComponentConfig componentRetreived;
            componentRetreived.FromString(val.as<String>());
            ConfigComponents.push_back(componentRetreived);
        }

        if (StationDatabaseId == 0)
        {
            throw std::runtime_error("Invalid deserialization");
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String StationConfigResponse::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["StationDatabaseId"] = StationDatabaseId;

    JsonArray array = json["ConfigComponents"].to<JsonArray>();

    for (ComponentConfig configComp : ConfigComponents)
    {
        JsonObject arrayItem = array.add<JsonObject>();
        configComp.ToJson(arrayItem);
    }

    String output;
    serializeJson(doc, output);
    return output;
}