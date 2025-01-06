#include <Acquisition/DhtRepository.h>
#include "DhtRepository.h"

using namespace SmartWeather::Repositories;
using namespace SmartWeather::Repositories::Acquisition;

DhtRepository::DhtRepository(BrokerService *brokerService)
    : _brokerService(brokerService),
      _dht(SmartWeather::Constants::DHT_PIN, DHT11)
{
    _dht.begin();
}

void DhtRepository::Acquire()
{
    float temperature = _dht.readTemperature();
    float humidity = _dht.readHumidity();

    if (isnan(temperature) || isnan(humidity))
    {
        Serial.println("Erreur lors de la lecture des données DHT.");
    }
    else
    {
        Serial.print("Température: ");
        Serial.println(temperature);
        Serial.print("Humidité: ");
        Serial.print(humidity);
        Serial.println("%");

        _brokerService->SendComponentDataSavingRequest(SmartWeather::Constants::DHT_PIN, _tempId, temperature);
        _brokerService->SendComponentDataSavingRequest(SmartWeather::Constants::DHT_PIN, _humidityId, humidity);
    }
}

PinConfig DhtRepository::GetConfig()
{
    PinConfig dhtConf;
    dhtConf.DefaultName = "DHT Sensor - Pin[" + String(SmartWeather::Constants::DHT_PIN) + "]";
    dhtConf.GpioPin = DHT_PIN;
    dhtConf.ComponentType = (int)ComponentType::Sensor;
    std::vector<MeasurePointConfig> configs;
    MeasurePointConfig tempMeasurePointConf;
    tempMeasurePointConf.DefaultName = "DHT - Temparature";
    tempMeasurePointConf.LocalId = _tempId;
    tempMeasurePointConf.Unit = (int)MeasureUnit::Celsius;
    configs.push_back(tempMeasurePointConf);
    MeasurePointConfig humidityMeasurePointConf;
    humidityMeasurePointConf.DefaultName = "DHT - Humidity";
    humidityMeasurePointConf.LocalId = _humidityId;
    humidityMeasurePointConf.Unit = (int)MeasureUnit::Percentage;
    configs.push_back(humidityMeasurePointConf);
    dhtConf.MeasurePoints = configs;

    return dhtConf;
}