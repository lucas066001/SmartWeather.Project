#include <Dtos/ActivationPlanConfig/ActivationPlanConfig.h>

using namespace SmartWeather::Entities::Dtos;

void ActivationPlanConfig::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        JsonArray array = json["ActivationPlanDatas"].as<JsonArray>();
        for (JsonVariant v : array)
        {
            ActivationPlanData apConfigRetreived;
            apConfigRetreived.FromString(v.as<String>());
            ActivationPlanDatas.push_back(apConfigRetreived);
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String ActivationPlanConfig::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    JsonArray array = json["ActivationPlanDatas"].to<JsonArray>();

    for (ActivationPlanData apConfigRetreived : ActivationPlanDatas)
    {
        JsonObject arrayItem = array.add<JsonObject>();
        apConfigRetreived.ToJson(arrayItem);
    }

    String output;
    serializeJson(doc, output);
    return output;
}
