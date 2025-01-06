#ifndef MQTT_RESPONSE_H
#define MQTT_RESPONSE_H

#include "ISerializableEntity.h"
#include "ArduinoJson.h"
#include "MqttHeader.h"

namespace SmartWeather::Entities::Contract {

    class MqttResponse : public ISerializableEntity {
    public:
        MqttHeader Header;
        int ExecutionResult;
        String ExecutionMessage;
        String JsonObject;
        int JsonLenght;
        int JsonType;

        void FromString(const String& jsonString) override;
        String ToString() override;

    };
}

#endif
