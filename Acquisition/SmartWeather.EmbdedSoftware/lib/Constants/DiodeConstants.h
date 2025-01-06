#ifndef DIODE_CONSTANTS_H
#define DIODE_CONSTANTS_H

#pragma once

#include <Arduino.h>

namespace SmartWeather::Constants {
    extern int BLUE_DIODE_PIN;
    extern int GREEN_DIODE_PIN;
    extern int RED_DIODE_PIN;

    enum class BoardState {
        PENDING,
        ERROR,
        OK,
        DOWN
    };
}

#endif
