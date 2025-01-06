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

        void Acquire() override;
        PinConfig GetConfig() override;

    private:
        BrokerService *_brokerService;
        int _gpioPin;
        int _defaultLocalId = 1;
    };
}

#endif
