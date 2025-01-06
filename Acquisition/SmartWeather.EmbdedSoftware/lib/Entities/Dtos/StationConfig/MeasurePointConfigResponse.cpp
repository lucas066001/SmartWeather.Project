#include <Dtos/StationConfig/MeasurePointConfigResponse.h>
#include "MeasurePointConfigResponse.h"

using namespace SmartWeather::Entities::Dtos;

void MeasurePointConfigResponse::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        Id = json["Id"].as<int>();
        DatabaseId = json["DatabaseId"].as<int>();
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String MeasurePointConfigResponse::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["Id"] = Id;
    json["DatabaseId"] = DatabaseId;

    String output;
    serializeJson(doc, output);
    return output;
}

void MeasurePointConfigResponse::ToJson(JsonObject &json)
{
    json["Id"] = Id;
    json["DatabaseId"] = DatabaseId;
}

void MeasurePointConfigResponse::FromJson(JsonObject &json)
{
    Id = json["Id"].as<int>();
    DatabaseId = json["DatabaseId"].as<int>();
}
