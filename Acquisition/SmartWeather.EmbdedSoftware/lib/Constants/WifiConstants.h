#ifndef WIFI_CONSTANTS_H
#define WIFI_CONSTANTS_H

#pragma once

#include <Arduino.h>

namespace SmartWeather::Constants {
    extern const char* DEFAULT_SSID;
    extern const char* DEFAULT_PASSWORD;
    extern const IPAddress LOCAL_IP;
    extern const IPAddress GATEWAY;
    extern const IPAddress SUBNET;

    extern String TARGET_WIFI_SSID;
    extern String TARGET_WIFI_PASSWORD;
}

#endif // WIFI_CONSTANTS_H
