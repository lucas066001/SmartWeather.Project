#include <Acquisition/AnalogRepository.h>

using namespace SmartWeather::Repositories;
using namespace SmartWeather::Repositories::Acquisition;

AnalogRepository::AnalogRepository(int gpioPin, BrokerService *brokerService)
    : _brokerService(brokerService),
      _taskHandle(nullptr),
      _running(false),
      _gpioPin(gpioPin)
{
    pinMode(_gpioPin, INPUT);
}

void AnalogRepository::AcquisitionTask(void *pvParameters)
{
    AnalogRepository *service = static_cast<AnalogRepository *>(pvParameters);
    service->AcquisitionLoop();
    vTaskDelete(NULL);
}

void AnalogRepository::StartAcquisition()
{
    if (_running)
    {
        Serial.println("Acquisition déjà en cours.");
        return;
    }

    _running = true;

    String taskName = "Analog Acquisition Task, PIN : " + String(_gpioPin);

    xTaskCreate(
        AnalogRepository::AcquisitionTask,
        taskName.c_str(),
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

void AnalogRepository::StopAcquisition()
{
    if (_running && _taskHandle != nullptr)
    {
        _running = false;

        vTaskDelete(_taskHandle);
        _taskHandle = nullptr;

        Serial.println("Acquisition arrêtée.");
    }
}

bool AnalogRepository::GetState()
{
    return _running;
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

void AnalogRepository::AcquisitionLoop()
{
    while (_running)
    {
        uint16_t value = analogRead(_gpioPin);

        if (isnan(value))
        {
            Serial.println("Erreur lors de la lecture des données GPIO PIN : " + String(_gpioPin));
        }
        else
        {
            Serial.print("GPIO PIN : " + String(_gpioPin));
            Serial.println(value);

            _brokerService->SendComponentDataSavingRequest(_gpioPin, _defaultLocalId, value);
        }

        vTaskDelay(pdMS_TO_TICKS(1000));
    }
}
