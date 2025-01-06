#ifndef ACTUATOR_COMMAND_REQUEST_H
#define ACTUATOR_COMMAND_REQUEST_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"

namespace SmartWeather::Entities::Dtos
{

    class ActuatorCommandRequest : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        int GpioPin;
        int Value;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
