#ifndef ACTIVATION_DATA_H
#define ACTIVATION_DATA_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"
#include <vector>

namespace SmartWeather::Entities::Dtos
{
    class ActivationPlanData : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        int ActuatorId;
        double TimeUntilNextActivation;
        double Duration;
        double Period;
        int NbCycles;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
