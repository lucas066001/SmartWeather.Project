#ifndef CONTRACT_CONSTANTS_H
#define CONTRACT_CONSTANTS_H

#pragma once

#include <Arduino.h>

namespace SmartWeather::Constants
{
    enum class Status
    {
        INTERNAL_ERROR,
        TIMEOUT_ERROR,
        DATABASE_ERROR,
        PARSE_ERROR,
        CONTRACT_ERROR,
        OK
    };

    enum class ComponentType
    {
        Unknown,
        Sensor,
        Actuator
    };

    enum class MeasureUnit
    {
        Unknown,
        Celsius,
        Percentage,
        UvStrength
    };
}

#endif
