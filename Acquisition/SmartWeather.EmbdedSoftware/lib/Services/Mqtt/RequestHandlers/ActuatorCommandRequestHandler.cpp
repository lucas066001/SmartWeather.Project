#include <Mqtt/RequestHandlers/ActuatorCommandRequestHandler.h>

using namespace SmartWeather::Services::Mqtt::RequestHandlers;

ActuatorCommandRequestHandler::ActuatorCommandRequestHandler(BrokerSingleton *brokerSingleton) : _brokerSingleton(brokerSingleton)
{
}

bool ActuatorCommandRequestHandler::IsAbleToHandle(const int requestType)
{
    return static_cast<JsonMessageType>(requestType) == JsonMessageType::ACTUATOR_REQUEST;
}

void ActuatorCommandRequestHandler::Handle(String &originTopic, MqttRequest &request)
{
    Serial.println("Handling actuator request");
    ActuatorCommandRequest actuatorRequest;
    try
    {
        actuatorRequest.FromString(request.JsonObject);
        Serial.println("Actuator request parsed");
        Serial.println("Pin target = " + String(actuatorRequest.GpioPin));

        for (IActuatorRepository *repo : _actuatorRepositories)
        {
            Serial.println("Repository pin to target = " + String(repo->GetPin()));

            if (repo->GetPin() == actuatorRequest.GpioPin)
            {
                Serial.println("Repository found");
                repo->Activate();
                _brokerSingleton->SendSuccessResponse(request,
                                                      originTopic,
                                                      "",
                                                      (int)JsonMessageType::ACTUATOR_RESPONSE);
                vTaskDelay(pdMS_TO_TICKS(actuatorRequest.Value));
                repo->DeActivate();

                return;
            }
        }
    }
    catch (const std::exception &e)
    {
        String requestEmitter = String(DefaultIdentifiers::DEFAULT_SENDER_ID);
        String requestIdentifier = String(DefaultIdentifiers::DEFAULT_REQUEST_ID);

        _brokerSingleton->SendErrorResponse(request.Header.RequestEmitter,
                                            request.Header.RequestIdentifier,
                                            originTopic,
                                            ExecutionResults::INTERNAL_ERROR,
                                            MqttBaseResponses::INTERNAL_ERROR);
        return;
    }
}

void ActuatorCommandRequestHandler::RegisterActuatorRepository(IActuatorRepository *actuatorRepository)
{
    _actuatorRepositories.push_back(actuatorRepository);
}
