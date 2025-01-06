#ifndef DIGITAL_ACTUATOR_REPOSITORY_H
#define DIGITAL_ACTUATOR_REPOSITORY_H

#include "Arduino.h"
#include <PinsConstants.h>
#include <ContractConstants.h>
#include <IActuatorRepository.h>
#include <Mqtt/BrokerService.h>

using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Constants;

namespace SmartWeather::Repositories::Actuator
{
    class DigitalActuatorRepository : public IActuatorRepository
    {
    public:
        DigitalActuatorRepository(int gpioPin, BrokerService *brokerService);

        void Activate() override;
        void DeActivate() override;
        int GetPin() override;
        bool GetState() override;
        PinConfig GetConfig() override;

    private:
        BrokerService *_brokerService;
        bool _state;
        int _gpioPin;
    };
}

#endif
