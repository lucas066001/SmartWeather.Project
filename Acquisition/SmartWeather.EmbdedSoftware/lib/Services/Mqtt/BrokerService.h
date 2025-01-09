#ifndef BROKER_SERVICE_H
#define BROKER_SERVICE_H

#include <Mqtt/BrokerSingleton.h>
#include <Mqtt/MqttConfigConstants.h>
#include <Mqtt/MqttContractConstants.h>
#include <CommonService.h>
#include <PinsConstants.h>
#include <vector>
#include <string>
#include <IAcquisitionRepository.h>
#include <IActuatorRepository.h>

using namespace SmartWeather::Repositories;

namespace SmartWeather::Services::Mqtt
{

    class BrokerService
    {
    public:
        BrokerService(BrokerSingleton *brokerSingleton,
                      CommonService &commonService,
                      BoardStateService &boardStateService,
                      ConnectionService &connectionService);

        int GetSationDatabseId();
        int GetCorrespondingMeasurePointDatabaseId(int gpioPin, int localMeasurePointId);
        void ConfigureStation();
        void LaunchAcquisition();
        bool IsStationConfigured();
        void SendComponentDataSavingRequest(int gpioPin, int measurePointId, float value);
        void RegisterAcquisitionRepo(IAcquisitionRepository *acquisitionRepository);
        void RegisterActuatorRepo(IActuatorRepository *acquisitionRepository);

    private:
        BrokerSingleton *_brokerSingleton;
        CommonService &_commonService;
        BoardStateService &_boardStateService;
        ConnectionService &_connectionService;

        std::vector<IAcquisitionRepository *> _acquisitionRepos;
        std::vector<IActuatorRepository *> _actuatorRepos;
    };
}

#endif
