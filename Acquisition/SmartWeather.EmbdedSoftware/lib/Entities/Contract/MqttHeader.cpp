#include <Contract/MqttHeader.h>
#include "MqttHeader.h"

using namespace SmartWeather::Entities::Contract;

void MqttHeader::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        JsonObject json = doc.as<JsonObject>();

        RequestEmitter = json["RequestEmitter"].as<String>();
        RequestIdentifier = json["RequestIdentifier"].as<String>();
        DateTime = json["DateTime"].as<String>();

        if (RequestEmitter.length() == 0 || RequestIdentifier.length() == 0)
        {
            throw std::runtime_error("Invalid deserialization");
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String MqttHeader::ToString()
{
    JsonDocument doc;
    JsonObject json = doc.to<JsonObject>();

    json["RequestEmitter"] = RequestEmitter;
    json["RequestIdentifier"] = RequestIdentifier;
    json["DateTime"] = DateTime;

    String output;
    serializeJson(doc, output);
    return output;
}

void MqttHeader::ToJson(JsonObject &json)
{
    json["RequestEmitter"] = RequestEmitter;
    json["RequestIdentifier"] = RequestIdentifier;
    json["DateTime"] = DateTime;
}

void MqttHeader::FromJson(JsonObject &json)
{
    RequestEmitter = json["RequestEmitter"].as<String>();
    RequestIdentifier = json["RequestIdentifier"].as<String>();
    DateTime = json["DateTime"].as<String>();
}
