#include <Arduino.h>
#include <TimeService.h>
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

TimeService timeService;
BoardStateService boardStateService;
ConnectionService connectionService(boardStateService);
AccessPointService accessPointService(boardStateService);
CommonService commonService(connectionService);
BrokerSingleton *brokerSingleton;
BrokerService *brokerService;
DhtRepository *dhtRepository;
AnalogRepository *uvRepository;
AnalogRepository *moistureRepository;
DigitalActuatorRepository *pumpRepository;
ActuatorCommandRequestHandler *actuatorRequestHandler;

bool connectionError = false;

void setup()
{
    Serial.begin(921600);
    boardStateService.SetState(BoardState::PENDING);

    // Provide wifi access point to user
    accessPointService.Start();

    // Initialize broker service with dependencies
    brokerSingleton = BrokerSingleton::GetInstance(&boardStateService,
                                                   &connectionService,
                                                   &timeService,
                                                   &commonService);

    brokerService = new BrokerService(brokerSingleton, commonService, boardStateService, connectionService);
    dhtRepository = new DhtRepository(brokerService);
    uvRepository = new AnalogRepository(UV_PIN, brokerService);
    moistureRepository = new AnalogRepository(MOISTURE_PIN, brokerService);
    pumpRepository = new DigitalActuatorRepository(BLUE_DIODE_PIN, brokerService);

    brokerService->RegisterAcquisitionRepo(dhtRepository);
    brokerService->RegisterAcquisitionRepo(uvRepository);
    brokerService->RegisterAcquisitionRepo(moistureRepository);
    brokerService->RegisterAcquisitionRepo(moistureRepository);
    brokerService->RegisterAcquisitionRepo(moistureRepository);
    brokerService->RegisterActuatorRepo(pumpRepository);

    actuatorRequestHandler = new ActuatorCommandRequestHandler(brokerSingleton);
    actuatorRequestHandler->RegisterActuatorRepository(pumpRepository);
    brokerSingleton->Handlers.push_back(actuatorRequestHandler);
}

void loop()
{
    // First, user needs to fill form
    // So the station will be able to connect to internet
    if (accessPointService.IsAvailable())
    {
        accessPointService.HandleClient();
    }

    // Once user filled registration form connection could be established
    if (Constants::TARGET_WIFI_PASSWORD != "" &&
        Constants::TARGET_WIFI_SSID != "" &&
        !connectionService.IsConnected())
    {
        accessPointService.Stop();
        connectionService.Connect();

        if (connectionService.IsConnected())
        {

            // Initialize timeservice for future needs
            timeService.Init();

            // Initialize Broker service to start publish / subscribe events
            brokerSingleton->Launch();

            // At this stage broker should be connected
            if (!brokerSingleton->IsConnected())
            {
                boardStateService.SetState(BoardState::ERROR);
                connectionError = true;
            }
            else
            {
                brokerService->ConfigureStation();
            }

            if (brokerService->IsStationConfigured())
            {
                Serial.println("Station is configured, should start acquisition");
                brokerService->LaunchAcquisition();
            }
        }
        else
        {
            boardStateService.SetState(BoardState::ERROR);
            connectionError = true;
        }
    }
}