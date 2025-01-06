#ifndef ACCESS_POINT_SERVICE_H
#define ACCESS_POINT_SERVICE_H

#pragma once

#include <WiFi.h>
#include <WebServer.h>
#include <WifiConstants.h>
#include <HomePage.h>
#include <BoardStateService.h>

namespace SmartWeather::Services
{

    class AccessPointService
    {
    public:
        AccessPointService(BoardStateService &boardStateService);

        void Start();
        void Stop();
        void HandleClient();
        bool IsAvailable();

    private:
        WebServer _server;
        bool _wifiStarted = false;
        BoardStateService &_boardStateService;

        void _initPages();
    };

}

#endif
