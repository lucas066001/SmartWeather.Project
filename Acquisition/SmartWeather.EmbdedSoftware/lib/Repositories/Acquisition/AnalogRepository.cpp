#include <Acquisition/AnalogRepository.h>

using namespace SmartWeather::Repositories;
using namespace SmartWeather::Repositories::Acquisition;

AnalogRepository::AnalogRepository(int gpioPin, BrokerService *brokerService)
    : _brokerService(brokerService),
      _gpioPin(gpioPin)
{
    pinMode(_gpioPin, INPUT);
}

void AnalogRepository::Acquire()
{
    uint16_t value = analogRead(_gpioPin);

    if (isnan(value))
    {
        Serial.println("Erreur lors de la lecture des données GPIO PIN : " + String(_gpioPin));
    }
    else
    {
        Serial.print("GPIO PIN [" + String(_gpioPin) + "] : ");
        Serial.println(value);
        _brokerService->SendComponentDataSavingRequest(_gpioPin, _defaultLocalId, value);
    }
}

PinConfig AnalogRepository::GetConfig()
{
    PinConfig analogPinConf;
    analogPinConf.DefaultName = "Analog Sensor - Pin[" + String(_gpioPin) + "]";
    analogPinConf.GpioPin = _gpioPin;
    analogPinConf.ComponentType = (int)ComponentType::Sensor;
    std::vector<MeasurePointConfig> configs;
    MeasurePointConfig tempMeasurePointConf;
    tempMeasurePointConf.DefaultName = "Measure point";
    tempMeasurePointConf.LocalId = _defaultLocalId;
    tempMeasurePointConf.Unit = (int)MeasureUnit::Unknown;
    configs.push_back(tempMeasurePointConf);
    analogPinConf.MeasurePoints = configs;

    return analogPinConf;
}