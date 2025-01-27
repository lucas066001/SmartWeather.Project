#include <Dtos/ActivationPlanConfig/ActivationPlanData.h>

using namespace SmartWeather::Entities::Dtos;

void ActivationPlanData::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        ActuatorId = json["ActuatorId"].as<int>();
        NbCycles = json["NbCycles"].as<int>();

        TimeUntilNextActivation = json["TimeUntilNextActivation"].as<double>();
        Duration = json["Duration"].as<double>();
        Period = json["Period"].as<double>();

        if (ActuatorId == 0)
        {
            throw std::runtime_error("Invalid deserialization");
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String ActivationPlanData::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    json["ActuatorId"] = ActuatorId;
    json["NbCycles"] = NbCycles;

    json["TimeUntilNextActivation"] = TimeUntilNextActivation;
    json["Duration"] = Duration;
    json["Period"] = Period;

    String output;
    serializeJson(doc, output);
    return output;
}

void ActivationPlanData::ToJson(JsonObject &json)
{
    json["ActuatorId"] = ActuatorId;
    json["NbCycles"] = NbCycles;

    json["TimeUntilNextActivation"] = TimeUntilNextActivation;
    json["Duration"] = Duration;
    json["Period"] = Period;
}

void ActivationPlanData::FromJson(JsonObject &json)
{
    ActuatorId = json["ActuatorId"].as<int>();
    NbCycles = json["NbCycles"].as<int>();

    TimeUntilNextActivation = json["TimeUntilNextActivation"].as<double>();
    Duration = json["Duration"].as<double>();
    Period = json["Period"].as<double>();
}
