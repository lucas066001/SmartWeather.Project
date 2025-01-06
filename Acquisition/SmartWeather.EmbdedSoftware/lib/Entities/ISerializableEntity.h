#ifndef ISERIALIZABLE_ENTITY_H
#define ISERIALIZABLE_ENTITY_H

#include <Arduino.h>

namespace SmartWeather::Entities {

    class ISerializableEntity {
    public:
        virtual void FromString(const String& jsonString) = 0;

        virtual String ToString() = 0;
    };
}
#endif