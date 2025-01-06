#ifndef DHT_REPOSITORY_H
#define DHT_REPOSITORY_H

#include "Arduino.h"
#include <DHT.h>
#include <ContractConstants.h>
#include <PinsConstants.h>
#include <IAcquisitionRepository.h>
#include <Mqtt/BrokerService.h>

using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Constants;

namespace SmartWeather::Repositories::Acquisition
{
    class DhtRepository : public IAcquisitionRepository
    {
    public:
        DhtRepository(BrokerService *brokerService);

        void StartAcquisition() override;
        void StopAcquisition() override;
        bool GetState() override;
        PinConfig GetConfig() override;

    private:
        static void AcquisitionTask(void *pvParameters);
        void AcquisitionLoop();

        BrokerService *_brokerService;
        TaskHandle_t _taskHandle;
        DHT _dht;
        bool _running;
        int _tempId = 1;
        int _humidityId = 2;
    };
}

#endif
