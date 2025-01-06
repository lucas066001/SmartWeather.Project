#ifndef ACTUATOR_COMMAND_REQUEST_HANDLER_H
#define ACTUATOR_COMMAND_REQUEST_HANDLER_H

#include <Arduino.h>
#include <vector>
#include <Mqtt/BrokerSingleton.h>
#include <Mqtt/IMqttRequestHandler.h>
#include <Mqtt/MqttContractConstants.h>
#include <IActuatorRepository.h>
#include <Dtos/ActuatorCommand/ActuatorCommandRequest.h>

using namespace SmartWeather::Repositories;
using namespace SmartWeather::Entities::Dtos;
using namespace SmartWeather::Constants::Mqtt;

namespace SmartWeather::Services::Mqtt::RequestHandlers
{

    class ActuatorCommandRequestHandler : public IMqttRequestHandler
    {
    public:
        ActuatorCommandRequestHandler(BrokerSingleton *brokerSingleton);

        bool IsAbleToHandle(const int requestType) override;
        void Handle(String &originTopic, MqttRequest &request) override;
        void RegisterActuatorRepository(IActuatorRepository *actuatorRepository);

    private:
        BrokerSingleton *_brokerSingleton;
        std::vector<IActuatorRepository *> _actuatorRepositories;
    };
}

#endif