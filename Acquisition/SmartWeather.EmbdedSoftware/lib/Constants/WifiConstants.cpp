#include "WifiConstants.h"

namespace SmartWeather::Constants {
    const char* DEFAULT_SSID = "SmartWeather.Station";
    const char* DEFAULT_PASSWORD = "12345678";
    const IPAddress LOCAL_IP(192, 168, 24, 12);
    const IPAddress GATEWAY(192, 168, 24, 1);
    const IPAddress SUBNET(255, 255, 255, 0);

    String TARGET_WIFI_SSID = "";
    String TARGET_WIFI_PASSWORD = "";
}