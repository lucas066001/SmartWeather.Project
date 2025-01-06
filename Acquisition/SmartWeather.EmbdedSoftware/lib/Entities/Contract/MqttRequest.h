#ifndef MQTT_REQUEST_H
#define MQTT_REQUEST_H

#include "ISerializableEntity.h"
#include "ArduinoJson.h"
#include "MqttHeader.h"

namespace SmartWeather::Entities::Contract {

    class MqttRequest : public ISerializableEntity {
    public:
        MqttHeader Header;
        String JsonObject;
        int JsonLenght;
        int JsonType;

        void FromString(const String& jsonString) override;
        String ToString() override;

    };
}

#endif
