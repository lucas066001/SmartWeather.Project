#include <Actuator/DigitalActuatorRepository.h>
#include "DigitalActuatorRepository.h"

using namespace SmartWeather::Repositories;
using namespace SmartWeather::Repositories::Actuator;

DigitalActuatorRepository::DigitalActuatorRepository(int gpioPin, BrokerService *brokerService)
    : _brokerService(brokerService),
      _state(false),
      _gpioPin(gpioPin)
{
    pinMode(_gpioPin, OUTPUT);
}

void DigitalActuatorRepository::Activate()
{
    digitalWrite(_gpioPin, HIGH);
    _state = true;
}

void Actuator::DigitalActuatorRepository::DeActivate()
{
    digitalWrite(_gpioPin, LOW);
    _state = false;
}

int SmartWeather::Repositories::Actuator::DigitalActuatorRepository::GetPin()
{
    return _gpioPin;
}

bool DigitalActuatorRepository::GetState()
{
    return _state;
}

PinConfig DigitalActuatorRepository::GetConfig()
{
    PinConfig actuatorConf;
    actuatorConf.DefaultName = "Digital actuator - Pin [" + String(_gpioPin) + "]";
    actuatorConf.GpioPin = _gpioPin;
    actuatorConf.ComponentType = (int)ComponentType::Actuator;
    return actuatorConf;
}
