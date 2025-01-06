#ifndef BROKER_SINGLETON_H
#define BROKER_SINGLETON_H

#include <WiFi.h>
#include <PubSubClient.h>
#include <future>
#include <vector>
#include <thread>
#include <pthread.h>
#include <chrono>
#include <iostream>
#include <algorithm>
#include <Arduino.h>
#include <PinsConstants.h>
#include <CommonService.h>
#include <BoardStateService.h>
#include <ConnectionService.h>
#include <Mqtt/MqttPendingRequest.h>
#include <Mqtt/MqttConfigConstants.h>
#include <Mqtt/IMqttRequestHandler.h>
#include <Mqtt/MqttContractConstants.h>
#include <Contract/MqttHeader.h>
#include <Contract/MqttRequest.h>
#include <Contract/MqttResponse.h>
#include <Dtos/StationConfig/ComponentConfig.h>
#include <Dtos/StationConfig/StationConfigRequest.h>
#include <Dtos/StationConfig/StationConfigResponse.h>

using namespace SmartWeather::Constants::Mqtt;
using namespace SmartWeather::Entities::Dtos;

namespace SmartWeather::Services::Mqtt
{

    class BrokerSingleton
    {
    public:
        static BrokerSingleton *GetInstance(BoardStateService *boardStateService = nullptr,
                                            ConnectionService *connectionService = nullptr,
                                            CommonService *commonService = nullptr);

        void Launch();
        bool IsConnected();
        void Subscribe(const char *topic);

        void SendRequest(String requestId, String targetTopic, String contentData, int objectType);
        void SendMessage(String targetTopic, String payload);
        void SendSuccessResponse(MqttRequest &request, String originTopic, String contentData, int objectType);
        void SendErrorResponse(String &requestEmitter, String &requestIdentifier, String originTopic, ExecutionResults errorCode = ExecutionResults::INTERNAL_ERROR, String customMessage = MqttBaseResponses::INTERNAL_ERROR);

        std::vector<MqttPendingRequestPromise *> PendingRequestPromise;
        std::vector<String *> Topics;
        std::vector<IMqttRequestHandler *> Handlers;
        StationConfigResponse *CurrentStationConfig = nullptr;

    private:
        PubSubClient _mqttClient;
        WiFiClient _wifiClient;
        static BrokerSingleton *_instance;
        BoardStateService &_boardStateService;
        ConnectionService &_connectionService;
        CommonService &_commonService;
        bool _connected;
        bool _acquireMessages;

        BrokerSingleton(BoardStateService &boardStateService,
                        ConnectionService &connectionService,
                        CommonService &commonService);
        ~BrokerSingleton();
        BrokerSingleton(const BrokerSingleton &) = delete;
        BrokerSingleton &operator=(const BrokerSingleton &) = delete;

        void _startAcquisition();
        void _stopAcquisition();
        void _connect();
        void _disconnect();
        static void _mqttMessageReceived(char *topic, byte *payload, unsigned int length);
    };

}

#endif
