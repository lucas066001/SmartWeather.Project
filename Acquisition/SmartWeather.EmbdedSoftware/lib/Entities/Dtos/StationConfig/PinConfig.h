#ifndef PIN_CONFIG_H
#define PIN_CONFIG_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"
#include <Dtos/StationConfig/MeasurePointConfig.h>
#include <vector>

namespace SmartWeather::Entities::Dtos
{

    class PinConfig : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        int GpioPin;
        int ComponentType;
        String DefaultName;
        std::vector<MeasurePointConfig> MeasurePoints;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
