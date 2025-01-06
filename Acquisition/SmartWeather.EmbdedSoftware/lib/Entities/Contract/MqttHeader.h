#ifndef MQTT_HEADER_H
#define MQTT_HEADER_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"

namespace SmartWeather::Entities::Contract
{

    class MqttHeader : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        String RequestEmitter;
        String RequestIdentifier;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
