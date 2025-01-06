#ifndef MEASURE_POINT_CONFIG_RESPONSE_H
#define MEASURE_POINT_CONFIG_RESPONSE_H

#include "ISerializableEntity.h"
#include "IJsonifyEntity.h"
#include "ArduinoJson.h"

namespace SmartWeather::Entities::Dtos
{

    class MeasurePointConfigResponse : public ISerializableEntity, public IJsonifyEntity
    {
    public:
        int Id;
        int DatabaseId;

        void FromString(const String &jsonString) override;
        String ToString() override;
        void ToJson(JsonObject &json) override;
        void FromJson(JsonObject &json) override;
    };
}

#endif
