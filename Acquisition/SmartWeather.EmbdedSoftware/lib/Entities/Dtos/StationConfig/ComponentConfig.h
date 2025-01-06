#ifndef COMPONENT_CONFIG_H
#define COMPONENT_CONFIG_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"
#include <Dtos/StationConfig/MeasurePointConfigResponse.h>
#include <vector>

namespace SmartWeather::Entities::Dtos
{

    class ComponentConfig : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        int GpioPin;
        int DatabaseId;
        std::vector<MeasurePointConfigResponse> MeasurePointsConfigs;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
