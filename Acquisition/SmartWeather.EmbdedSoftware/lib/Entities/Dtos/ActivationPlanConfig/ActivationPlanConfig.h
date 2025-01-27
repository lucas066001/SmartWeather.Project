#ifndef ACTIVATION_PLAN_CONFIG_H
#define ACTIVATION_PLAN_CONFIG_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"
#include <Dtos/ActivationPlanConfig/ActivationPlanData.h>
#include <vector>

namespace SmartWeather::Entities::Dtos
{

    class ActivationPlanConfig : public ISerializableEntity
    {
    public:
        std::vector<ActivationPlanData> ActivationPlanDatas;

        void FromString(const String &jsonString) override;
        String ToString() override;
    };
}

#endif
