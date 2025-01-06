#ifndef IJSONIFY_ENTITY_H
#define IJSONIFY_ENTITY_H

#include <Arduino.h>
#include <ArduinoJson.h>

namespace SmartWeather::Entities
{

    class IJsonifyEntity
    {
    public:
        virtual void ToJson(JsonObject &json) = 0;
        virtual void FromJson(JsonObject &json) = 0;
    };
}
#endif