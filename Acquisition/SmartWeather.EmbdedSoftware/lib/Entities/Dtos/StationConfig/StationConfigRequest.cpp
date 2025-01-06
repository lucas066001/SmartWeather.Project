#include <Dtos/StationConfig/StationConfigRequest.h>

using namespace SmartWeather::Entities::Dtos;

void StationConfigRequest::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        MacAddress = json["MacAddress"].as<String>();

        JsonArray array = json["ComponentsConfigs"].as<JsonArray>();
        for (JsonVariant v : array)
        {
            PinConfig pinConfigRetreived;
            pinConfigRetreived.FromString(v.as<String>());
            ComponentsConfigs.push_back(pinConfigRetreived);
        }

        if (MacAddress.length() == 0)
        {
            throw std::runtime_error("Invalid deserialization");
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String StationConfigRequest::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["MacAddress"] = MacAddress;

    JsonArray array = json["ComponentsConfigs"].to<JsonArray>();

    for (PinConfig pinConf : ComponentsConfigs)
    {
        JsonObject arrayItem = array.add<JsonObject>();
        pinConf.ToJson(arrayItem);
    }

    String output;
    serializeJson(doc, output);
    return output;
}