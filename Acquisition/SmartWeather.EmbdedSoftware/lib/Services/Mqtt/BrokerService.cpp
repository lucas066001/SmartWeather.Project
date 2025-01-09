#include <Mqtt/BrokerService.h>

using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Constants::Mqtt;
using namespace SmartWeather::Constants;

BrokerService::BrokerService(BrokerSingleton *brokerSingleton,
                             CommonService &commonService,
                             BoardStateService &boardStateService,
                             ConnectionService &connectionService) : _brokerSingleton(brokerSingleton),
                                                                     _commonService(commonService),
                                                                     _boardStateService(boardStateService),
                                                                     _connectionService(connectionService)
{
}

int BrokerService::GetSationDatabseId()
{
    if (IsStationConfigured())
    {
        return _brokerSingleton->CurrentStationConfig->StationDatabaseId;
    }
    else
    {
        return 0;
    }
}

int BrokerService::GetCorrespondingMeasurePointDatabaseId(int gpioPin, int localMeasurePointId)
{
    if (IsStationConfigured())
    {
        for (auto configComponent : _brokerSingleton->CurrentStationConfig->ConfigComponents)
        {
            if (configComponent.GpioPin == gpioPin)
            {
                for (MeasurePointConfigResponse mpConf : configComponent.MeasurePointsConfigs)
                {
                    if (mpConf.Id == localMeasurePointId)
                    {
                        return mpConf.DatabaseId;
                    }
                }
            }
        }
        return 0;
    }
    else
    {
        return 0;
    }
}

void BrokerService::ConfigureStation()
{
    // First we prepare the necessary content for station conf request
    StationConfigRequest dataContent;
    std::vector<PinConfig> stationPinsConfig;

    for (IAcquisitionRepository *repo : _acquisitionRepos)
    {
        stationPinsConfig.push_back(repo->GetConfig());
    }

    for (IActuatorRepository *repo : _actuatorRepos)
    {
        stationPinsConfig.push_back(repo->GetConfig());
    }

    dataContent.MacAddress = _connectionService.GetCurrentMacAddress();
    dataContent.ComponentsConfigs = stationPinsConfig;

    // Then we create a pending request that we will wait for afterward
    String requestId = _commonService.GenerateGuid();
    std::promise<MqttResponse> promise;
    std::future<MqttResponse> futureResponse = promise.get_future();
    MqttPendingRequestPromise *currentRequestPromise = new MqttPendingRequestPromise(requestId, promise);

    // We register this request in the singleton so the response handler can find it
    _brokerSingleton->PendingRequestPromise.push_back(currentRequestPromise);

    // We send the request on the corresponding topic
    String configTopicRequest = String(MQTT_CONFIG_TOPIC) + String(MQTT_SERVER_TARGET);

    _brokerSingleton->SendRequest(requestId,
                                  configTopicRequest,
                                  dataContent.ToString(),
                                  static_cast<int>(JsonMessageType::CONFIG_REQUEST));

    try
    {
        Serial.println("Waiting for response...");
        // We wait for the response

        if (futureResponse.wait_for(std::chrono::seconds(10)) == std::future_status::ready)
        {
            Serial.println("Response arrived !!!");

            MqttResponse response = futureResponse.get();

            if (response.ExecutionResult == static_cast<int>(ExecutionResults::OK))
            {
                Serial.println("Response is ok");
                StationConfigResponse *receivedConf = new StationConfigResponse();
                Serial.println("Trying to deserialize conf");
                receivedConf->FromString(response.JsonObject);
                Serial.println("Received configuration looks like this : " + receivedConf->ToString());

                // We make sure values are coherents
                if (!(receivedConf->StationDatabaseId >= 0) ||
                    !(receivedConf->ConfigComponents.size() >= 0))
                {
                    delete receivedConf;
                    Serial.println("Invalid values");
                    _boardStateService.SetState(BoardState::ERROR);
                    throw std::runtime_error("Invalid received configuration");
                }

                // We save the config into the singleton so other service can access it
                _brokerSingleton->CurrentStationConfig = receivedConf;
                Serial.println("New config installed !!!!");

                String *configTopic = new String(MQTT_ACTUATOR_TOPIC_FORMAT);
                configTopic->concat(MQTT_STATION_TARGET);
                configTopic->replace("{0}", String(receivedConf->StationDatabaseId));
                _brokerSingleton->Topics.clear();
                _brokerSingleton->Topics.push_back(configTopic);
                _brokerSingleton->Subscribe(configTopic->c_str());
            }
        }
        else
        {
            Serial.println("Timeout dépassé. La tâche n'est pas terminée.");
        }
    }
    catch (const std::exception &ex)
    {
        Serial.println("Station not configured");
    }

    if (!IsStationConfigured())
    {
        Serial.println("Error unable to configure station");
        _boardStateService.SetState(BoardState::ERROR);
    }
}

void BrokerService::LaunchAcquisition()
{
    while (true)
    {
        for (IAcquisitionRepository *repo : _acquisitionRepos)
        {
            repo->Acquire();
        }
        delay(5000);
    }
}

bool BrokerService::IsStationConfigured()
{
    return _brokerSingleton->CurrentStationConfig != nullptr;
}

void BrokerService::SendComponentDataSavingRequest(int gpioPin, int measurePointLocalId, float value)
{
    // Need to properly format topic
    String measureDataSavingTopicRequest = String(MQTT_SENSOR_TOPIC_FORMAT) + String(MQTT_SERVER_TARGET);
    measureDataSavingTopicRequest.replace("{0}", String(_brokerSingleton->CurrentStationConfig->StationDatabaseId));
    measureDataSavingTopicRequest.replace("{1}", String(GetCorrespondingMeasurePointDatabaseId(gpioPin, measurePointLocalId)));

    _brokerSingleton->SendMessage(measureDataSavingTopicRequest,
                                  String(value, 2));
    _commonService.LogHeap();
}

void BrokerService::RegisterAcquisitionRepo(IAcquisitionRepository *acquisitionRepository)
{
    _acquisitionRepos.push_back(acquisitionRepository);
}

void BrokerService::RegisterActuatorRepo(IActuatorRepository *actuatorRepository)
{
    _actuatorRepos.push_back(actuatorRepository);
}