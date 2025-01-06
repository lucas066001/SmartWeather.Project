#include "TimeService.h"
#include <Arduino.h>
#include <TimeLib.h>  

using namespace SmartWeather::Services;

// Prepare connection to time server
TimeService::TimeService()
    : _timeClient(_ntpUDP, "pool.ntp.org", 0, 60000)
{
}

// Initialize connection to time server
void TimeService::Init() 
{
    _timeClient.begin();
    while(!_timeClient.update()) 
    {
        _timeClient.forceUpdate(); 
    }
}

// Provide easy access to current datetime
String TimeService::GetCurrentDateTime() 
{
    unsigned long epochTime = _timeClient.getEpochTime();
    return _formatDateTime(epochTime);
}

// Formatting implementation
String TimeService::_formatDateTime(unsigned long epochTime) 
{
    int year = ::year(epochTime);
    int month = ::month(epochTime);
    int day = ::day(epochTime);
    int hour = ::hour(epochTime);
    int minute = ::minute(epochTime);
    int second = ::second(epochTime);

    char buffer[30];
    sprintf(buffer, "%04d-%02d-%02dT%02d:%02d:%02dZ", year, month, day, hour, minute, second);
    return String(buffer);
}
