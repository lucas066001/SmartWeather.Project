#ifndef MOISTURE_REPOSITORY_H
#define MOISTURE_REPOSITORY_H

#include "Arduino.h"
#include <ContractConstants.h>
#include <PinsConstants.h>
#include <IAcquisitionRepository.h>
#include <Mqtt/BrokerService.h>

using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Constants;

namespace SmartWeather::Repositories::Acquisition
{
    class AnalogRepository : public IAcquisitionRepository
    {
    public:
        AnalogRepository(int gpioPin, BrokerService *brokerService);

        void StartAcquisition() override;
        void StopAcquisition() override;
        bool GetState() override;
        PinConfig GetConfig() override;

    private:
        static void AcquisitionTask(void *pvParameters);
        void AcquisitionLoop();

        BrokerService *_brokerService;
        TaskHandle_t _taskHandle;
        bool _running;
        int _gpioPin;
        int _defaultLocalId = 1;
    };
}

#endif
