#include <Acquisition/DhtRepository.h>

using namespace SmartWeather::Repositories;
using namespace SmartWeather::Repositories::Acquisition;

DhtRepository::DhtRepository(BrokerService *brokerService)
    : _brokerService(brokerService),
      _dht(SmartWeather::Constants::DHT_PIN, DHT11),
      _taskHandle(nullptr),
      _running(false)
{
    _dht.begin();
}

void DhtRepository::AcquisitionTask(void *pvParameters)
{
    DhtRepository *service = static_cast<DhtRepository *>(pvParameters);
    service->AcquisitionLoop();
    vTaskDelete(NULL);
}

void DhtRepository::StartAcquisition()
{
    if (_running)
    {
        Serial.println("Acquisition déjà en cours.");
        return;
    }

    _running = true;

    xTaskCreate(
        DhtRepository::AcquisitionTask,
        "DHT Acquisition Task",
        8192,
        this,
        1,
        &_taskHandle);

    if (_taskHandle == nullptr)
    {
        Serial.println("Erreur lors de la création de la tâche d'acquisition.");
        _running = false;
    }
}

void DhtRepository::StopAcquisition()
{
    if (_running && _taskHandle != nullptr)
    {
        _running = false;

        vTaskDelete(_taskHandle);
        _taskHandle = nullptr;

        Serial.println("Acquisition arrêtée.");
    }
}

bool DhtRepository::GetState()
{
    return _running;
}

PinConfig DhtRepository::GetConfig()
{
    PinConfig dhtConf;
    dhtConf.DefaultName = "DHT Sensor";
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

void DhtRepository::AcquisitionLoop()
{
    while (_running)
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

        vTaskDelay(pdMS_TO_TICKS(1000));
    }
}
