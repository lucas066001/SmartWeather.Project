#include "ConnectionService.h"

using namespace SmartWeather::Services;
using namespace SmartWeather::Constants;

ConnectionService::ConnectionService(BoardStateService &boardStateService)
    : boardStateService(boardStateService) {}

bool ConnectionService::Connect()
{
    boardStateService.SetState(BoardState::PENDING);

    WiFi.mode(WIFI_STA);
    WiFi.begin(TARGET_WIFI_SSID.c_str(), TARGET_WIFI_PASSWORD.c_str());

    int attempts = 0;
    while (WiFi.status() != WL_CONNECTED && attempts < 10)
    {
        delay(1000);
        attempts++;
    }

    if (WiFi.status() == WL_CONNECTED)
    {
        boardStateService.SetState(BoardState::OK);
        return true;
    }
    else
    {
        boardStateService.SetState(BoardState::ERROR);
        Serial.println("ConnectionService.Connect -> unable to connect");
        return false;
    }
}

void ConnectionService::Disconnect()
{
    boardStateService.SetState(BoardState::PENDING);
    WiFi.disconnect();
    boardStateService.SetState(BoardState::OK);
}

bool ConnectionService::IsConnected()
{
    return WiFi.status() == WL_CONNECTED;
}

String ConnectionService::GetCurrentMacAddress()
{
    if (IsConnected())
    {
        return WiFi.macAddress();
    }
    return String();
}
