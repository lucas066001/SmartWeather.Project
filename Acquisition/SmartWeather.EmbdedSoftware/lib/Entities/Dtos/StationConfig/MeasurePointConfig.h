#ifndef MEASURE_POINT_CONFIG_H
#define MEASURE_POINT_CONFIG_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"

namespace SmartWeather::Entities::Dtos
{

    class MeasurePointConfig : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        int LocalId;
        String DefaultName;
        int Unit;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
