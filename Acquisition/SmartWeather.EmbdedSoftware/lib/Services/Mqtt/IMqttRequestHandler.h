#ifndef IMQTT_REQUEST_HANDLER_H
#define IMQTT_REQUEST_HANDLER_H

#include <Arduino.h>
#include <Mqtt/BrokerSingleton.h>
#include <Contract/MqttRequest.h>

using namespace SmartWeather::Entities::Contract;

namespace SmartWeather::Services::Mqtt {

    class IMqttRequestHandler {
    public:
        virtual bool IsAbleToHandle(const int requestType) = 0;

        virtual void Handle(String& originTopic, MqttRequest& request) = 0;
    };
}
#endif