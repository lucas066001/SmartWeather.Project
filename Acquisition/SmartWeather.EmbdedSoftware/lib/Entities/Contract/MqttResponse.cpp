#include <Contract/MqttResponse.h>

using namespace SmartWeather::Entities::Contract;

void MqttResponse::FromString(const String &jsonString)
{
    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, jsonString);

    if (!error)
    {
        ArduinoJson::V710PB22::JsonObject json = doc.as<ArduinoJson::V710PB22::JsonObject>();

        ArduinoJson::V710PB22::JsonObject headerJson = json["Header"].as<ArduinoJson::V710PB22::JsonObject>();
        Header.FromJson(headerJson);
        JsonObject = json["JsonObject"].as<String>();
        ExecutionMessage = json["ExecutionMessage"].as<String>();
        ExecutionResult = json["ExecutionResult"].as<int>();
        JsonLenght = json["JsonLenght"].as<int>();
        JsonType = json["JsonType"].as<int>();

        if (JsonObject.length() == 0 || JsonObject.length() != JsonLenght)
        {
            throw std::runtime_error("Invalid deserialization");
        }
    }
    else
    {
        throw std::runtime_error("Unable to deserialize object");
    }
}

String MqttResponse::ToString()
{
    JsonDocument doc;
    ArduinoJson::V710PB22::JsonObject json = doc.to<ArduinoJson::V710PB22::JsonObject>();

    ArduinoJson::V710PB22::JsonObject headerJson = json["Header"].to<ArduinoJson::V710PB22::JsonObject>();
    Header.ToJson(headerJson);
    json["JsonObject"] = JsonObject;
    json["ExecutionMessage"] = ExecutionMessage;
    json["ExecutionResult"] = ExecutionResult;
    json["JsonLenght"] = JsonLenght;
    json["JsonType"] = JsonType;

    String output;
    serializeJson(doc, output);
    return output;
}