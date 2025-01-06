#ifndef CONNECTION_SERVICE_H
#define CONNECTION_SERVICE_H

#include <WiFi.h>
#include <WifiConstants.h>
#include <BoardStateService.h>

namespace SmartWeather::Services
{

    class ConnectionService
    {
    public:
        ConnectionService(BoardStateService &boardStateService);

        bool Connect();
        void Disconnect();
        bool IsConnected();
        String GetCurrentMacAddress();

    private:
        BoardStateService &boardStateService;
    };

}

#endif
