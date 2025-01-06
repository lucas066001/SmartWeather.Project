#include <AccessPointService.h>

using namespace SmartWeather::Services;

AccessPointService::AccessPointService(BoardStateService &boardStateService)
    : _server(80),
      _boardStateService(boardStateService) {}

void AccessPointService::Start()
{
    // Configure access point credentials
    WiFi.softAP(SmartWeather::Constants::DEFAULT_SSID,
                SmartWeather::Constants::DEFAULT_PASSWORD);

    // Configure access point static ip
    WiFi.softAPConfig(SmartWeather::Constants::LOCAL_IP,
                      SmartWeather::Constants::GATEWAY,
                      SmartWeather::Constants::SUBNET);

    // Add urls handlers
    _initPages();

    // Start the server
    _server.begin();
    _boardStateService.SetState(SmartWeather::Constants::BoardState::PENDING);
    _wifiStarted = true;
}

void AccessPointService::Stop()
{
    // Stop the access point emission
    _server.stop();
    WiFi.softAPdisconnect(true);

    _boardStateService.SetState(SmartWeather::Constants::BoardState::PENDING);
    _wifiStarted = false;
}

void AccessPointService::HandleClient()
{
    // Start listing to requests
    _server.handleClient();
}

bool AccessPointService::IsAvailable()
{
    return _wifiStarted;
}

void AccessPointService::_initPages()
{
    // Display home page
    _server.on("/", [this]()
               { _server.send(200, "text/html", SmartWeather::WebPages::GetHomePage()); });

    // Handle saving request
    _server.on("/SaveParams", HTTP_POST, [this]()
               {
        if (_server.hasArg("ssid") && _server.hasArg("password")) 
        {
            SmartWeather::Constants::TARGET_WIFI_SSID = _server.arg("ssid");
            SmartWeather::Constants::TARGET_WIFI_PASSWORD = _server.arg("password");
            _server.send(200, "text/html", SmartWeather::WebPages::GetSuccessPage());
        } 
        else 
        {
            _server.send(400, "text/html", SmartWeather::WebPages::GetErrorMessagePage("Missing SSID or Password"));
        } });
}
