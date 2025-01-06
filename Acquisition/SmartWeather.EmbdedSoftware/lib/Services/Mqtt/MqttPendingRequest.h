#ifndef MQTT_PENDING_REQUEST_H
#define MQTT_PENDING_REQUEST_H

#include <Arduino.h>
#include <Mqtt/BrokerSingleton.h>
#include <Contract/MqttResponse.h>

using namespace SmartWeather::Entities::Contract;

namespace SmartWeather::Services::Mqtt {

    struct MqttPendingRequestPromise {
        const String RequestIdentifier;
        std::promise<MqttResponse>& CompletionRef;

        MqttPendingRequestPromise(String& requestIdentifier,
                                    std::promise<MqttResponse>& completionRef)
                                    : RequestIdentifier(requestIdentifier),
                                      CompletionRef(completionRef) {}
    };

}
#endif