#include <Arduino.h>
#include <CommonService.h>
#include <ConnectionService.h>
#include <BoardStateService.h>
#include <AccessPointService.h>
#include <Mqtt/BrokerSingleton.h>
#include <Mqtt/BrokerService.h>
#include <Mqtt/RequestHandlers/ActuatorCommandRequestHandler.h>
#include <Acquisition/DhtRepository.h>
#include <Acquisition/AnalogRepository.h>
#include <Actuator/DigitalActuatorRepository.h>

using namespace SmartWeather;
using namespace SmartWeather::Constants;
using namespace SmartWeather::Services;
using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Repositories;
using namespace SmartWeather::Repositories::Acquisition;
using namespace SmartWeather::Repositories::Actuator;
using namespace SmartWeather::Services::Mqtt::RequestHandlers;

BoardStateService boardStateService;
ConnectionService connectionService(boardStateService);
AccessPointService accessPointService(boardStateService);
CommonService commonService(connectionService, boardStateService);
BrokerSingleton *brokerSingleton;

void setup()
{
    Serial.begin(921600);
    boardStateService.SetState(BoardState::PENDING);

    // Provide wifi access point to user
    accessPointService.Start();

    // Initialize broker service with dependencies
    brokerSingleton = BrokerSingleton::GetInstance(&boardStateService,
                                                   &connectionService,
                                                   &commonService);
}

void loop()
{
    // First, user needs to fill form
    // So the station will be able to connect to internet
    boardStateService.SetState(BoardState::PENDING);
    while (accessPointService.IsAvailable())
    {
        accessPointService.HandleClient();
        if (Constants::TARGET_WIFI_PASSWORD != "" && Constants::TARGET_WIFI_SSID != "")
        {
            accessPointService.Stop();
            boardStateService.BlinkState(BoardState::OK);
        }
    }

    connectionService.Connect();

    // If unable to connect, restart process is needed
    if (!connectionService.IsConnected())
    {
        commonService.BlockBoardError();
    }

    // Acquisition process can now be launched
    BrokerService brokerService(brokerSingleton, commonService, boardStateService, connectionService);

    DhtRepository dhtRepository(&brokerService);
    AnalogRepository uvRepository(UV_PIN, &brokerService);
    AnalogRepository moistureRepository(MOISTURE_PIN, &brokerService);
    DigitalActuatorRepository pumpRepository(BLUE_DIODE_PIN, &brokerService);

    brokerService.RegisterAcquisitionRepo(&dhtRepository);
    brokerService.RegisterAcquisitionRepo(&uvRepository);
    brokerService.RegisterAcquisitionRepo(&moistureRepository);
    brokerService.RegisterAcquisitionRepo(&moistureRepository);
    brokerService.RegisterAcquisitionRepo(&moistureRepository);
    brokerService.RegisterActuatorRepo(&pumpRepository);

    ActuatorCommandRequestHandler actuatorRequestHandler(brokerSingleton);

    actuatorRequestHandler.RegisterActuatorRepository(&pumpRepository);
    brokerSingleton->Handlers.push_back(&actuatorRequestHandler);

    // Initialize Broker service to start publish / subscribe events
    brokerSingleton->Launch();

    // At this stage broker should be connected
    if (!brokerSingleton->IsConnected())
    {
        commonService.BlockBoardError();
    }

    brokerService.ConfigureStation();

    if (!brokerService.IsStationConfigured())
    {
        Serial.println("Station is configured, should start acquisition");
        commonService.BlockBoardError();
    }

    brokerService.LaunchAcquisition();
}