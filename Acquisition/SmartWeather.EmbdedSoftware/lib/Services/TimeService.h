#ifndef TIMESERVICE_H
#define TIMESERVICE_H

#include <WiFi.h>
#include <WiFiUdp.h>
#include <NTPClient.h>
#include <string>

namespace SmartWeather::Services {

    class TimeService {
    public:
        TimeService();
        void Init();
        String GetCurrentDateTime();

    private:
        WiFiUDP _ntpUDP;
        NTPClient _timeClient;

        String _formatDateTime(unsigned long epochTime);
    };

}
#endif
