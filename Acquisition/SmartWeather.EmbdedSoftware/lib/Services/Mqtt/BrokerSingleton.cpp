#include <Mqtt/BrokerSingleton.h>
#include "BrokerSingleton.h"

using namespace SmartWeather::Services;
using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Constants;
using namespace SmartWeather::Constants::Mqtt;
using namespace SmartWeather::Entities::Contract;
using namespace SmartWeather::Entities::Dtos;

BrokerSingleton *BrokerSingleton::_instance = nullptr;

BrokerSingleton *BrokerSingleton::GetInstance(BoardStateService *boardStateService,
                                              ConnectionService *connectionService,
                                              TimeService *timeService,
                                              CommonService *commonService)
{
    if (_instance == nullptr)
    {
        _instance = new BrokerSingleton(*boardStateService,
                                        *connectionService,
                                        *timeService,
                                        *commonService);
    }
    return _instance;
}

BrokerSingleton::BrokerSingleton(BoardStateService &boardStateService,
                                 ConnectionService &connectionService,
                                 TimeService &timeService,
                                 CommonService &commonService)
    : _boardStateService(boardStateService),
      _connectionService(connectionService),
      _timeService(timeService),
      _commonService(commonService),
      _mqttClient(_wifiClient)

{
    _mqttClient.setKeepAlive(60);
    _acquireMessages = false;
    String *configTopic = new String(MQTT_CONFIG_TOPIC);
    configTopic->concat(MQTT_STATION_TARGET);
    Topics.push_back(configTopic);
}

BrokerSingleton::~BrokerSingleton()
{
    _disconnect();
    delete _instance;
}

void BrokerSingleton::Launch()
{
    _connect();
    if (_connected)
    {
        xTaskCreate(
            [](void *param)
            {
                // Appel de la méthode _startAcquisition
                static_cast<BrokerSingleton *>(param)->_startAcquisition();
                vTaskDelete(NULL); // On supprime la tâche une fois terminée
            },
            "MQTT_Acquisition", // Nom de la tâche
            16384,              // Taille de la pile (peut être ajustée)
            this,               // Paramètre (instance actuelle)
            1,                  // Priorité de la tâche
            NULL                // Handle de la tâche (pas nécessaire ici)
        );

        // std::thread tAcquisition(std::bind(&BrokerSingleton::_startAcquisition, this));

        // tAcquisition.detach();
        Serial.println("Mqtt loop started");
    }
}

bool BrokerSingleton::IsConnected()
{
    return _connected;
}

void BrokerSingleton::SendRequest(String requestId,
                                  String targetTopic,
                                  String contentData,
                                  int objectType)
{
    Serial.println("Trying to send request on " + targetTopic);
    Serial.println("With content" + contentData);
    if (_connected)
    {
        // Generate proper request
        MqttHeader mqttHeader;
        mqttHeader.DateTime = _timeService.GetCurrentDateTime();
        mqttHeader.RequestEmitter = _connectionService.GetCurrentMacAddress();
        mqttHeader.RequestIdentifier = requestId;

        MqttRequest mqttRequest;
        mqttRequest.Header = mqttHeader;
        mqttRequest.JsonObject = contentData;
        mqttRequest.JsonLenght = contentData.length();
        mqttRequest.JsonType = objectType;

        Serial.println("Request looks like this :" + mqttRequest.ToString());

        bool isSent = _mqttClient.publish(targetTopic.c_str(), mqttRequest.ToString().c_str());
        if (isSent)
        {
            Serial.print("Message publié sur le topic: ");
            Serial.println(targetTopic);
        }
        else
        {
            Serial.print("Message impossible à envoyer ");
            Serial.println(_mqttClient.state());
        }
    }
    else
    {
        Serial.print("Error unable to send request ");
        _boardStateService.SetState(BoardState::ERROR);
    }
}

void BrokerSingleton::SendMessage(String targetTopic, String payload)
{
    if (_connected)
    {
        bool isSent = false;
        try
        {
            isSent = _mqttClient.publish(targetTopic.c_str(), payload.c_str());
        }
        catch (const std::exception &e)
        {
            Serial.print("Message impossible à envoyer ");
            Serial.println(_mqttClient.state());
        }

        if (isSent)
        {
            Serial.print("Message publié sur le topic: ");
            Serial.println(targetTopic);
        }
        else
        {
            Serial.print("Message impossible à envoyer ");
            Serial.println(_mqttClient.state());
        }
    }
    else
    {
        Serial.print("Error unable to send request, not connected ");
        _boardStateService.SetState(BoardState::ERROR);
    }
}

void BrokerSingleton::SendSuccessResponse(MqttRequest &request,
                                          String originTopic,
                                          String contentData,
                                          int objectType)
{
    if (_connected)
    {
        // Generate proper response
        MqttHeader mqttHeader;
        mqttHeader.DateTime = _timeService.GetCurrentDateTime();
        mqttHeader.RequestEmitter = request.Header.RequestEmitter;
        mqttHeader.RequestIdentifier = request.Header.RequestIdentifier;

        MqttResponse mqttResponse;
        mqttResponse.Header = mqttHeader;
        mqttResponse.ExecutionMessage = MqttBaseResponses::OK;
        mqttResponse.ExecutionResult = static_cast<int>(ExecutionResults::OK);
        mqttResponse.JsonObject = contentData;
        mqttResponse.JsonLenght = contentData.length();
        mqttResponse.JsonType = objectType;

        // Transform orgin topic to target server
        originTopic.replace(MQTT_STATION_TARGET, MQTT_SERVER_TARGET);

        _mqttClient.publish(originTopic.c_str(), mqttResponse.ToString().c_str());
        Serial.print("Message publié sur le topic: ");
        Serial.println(originTopic);
    }
    else
    {
        Serial.print("Error unable to send success message");
        _boardStateService.SetState(BoardState::ERROR);
    }
}

void BrokerSingleton::SendErrorResponse(String &requestEmitter,
                                        String &requestIdentifier,
                                        String originTopic,
                                        ExecutionResults errorCode,
                                        String customMessage)
{
    if (_connected)
    {
        // Generate proper response
        MqttHeader mqttHeader;
        mqttHeader.DateTime = _timeService.GetCurrentDateTime();
        mqttHeader.RequestEmitter = requestEmitter;
        mqttHeader.RequestIdentifier = requestIdentifier;

        MqttResponse mqttResponse;
        mqttResponse.Header = mqttHeader;
        mqttResponse.ExecutionMessage = customMessage;
        mqttResponse.ExecutionResult = static_cast<int>(errorCode);
        mqttResponse.JsonLenght = 0;
        mqttResponse.JsonType = static_cast<int>(JsonMessageType::UNKNOWN);

        // Transform orgin topic to target server
        originTopic.replace(MQTT_STATION_TARGET, MQTT_SERVER_TARGET);

        _mqttClient.publish(originTopic.c_str(), mqttResponse.ToString().c_str());
        Serial.print("Message publié sur le topic: ");
        Serial.println(originTopic);
    }
    else
    {
        Serial.print("Error SendErrorResponse");
        _boardStateService.SetState(BoardState::ERROR);
    }
}

void BrokerSingleton::Subscribe(const char *topic)
{
    if (_connected)
    {
        bool subscribeSuccess = _mqttClient.subscribe(topic);
        if (subscribeSuccess)
        {
            Serial.print("Souscrit au topic: ");
            Serial.println(topic);
        }
        else
        {
            Serial.print("IMPOSSIBLE DE SOUSCRIRE AU TOPIC: ");
            Serial.println(topic);
        }
    }
    else
    {
        Serial.println("Impossible de souscrire, non connecté au broker.");
    }
}

void BrokerSingleton::_connect()
{
    _boardStateService.SetState(BoardState::PENDING);

    if (!MQTT_IP || MQTT_PORT == 0 || !MQTT_CLIENT_ID)
    {
        Serial.println("Erreur de configuration MQTT.");
        _boardStateService.SetState(BoardState::ERROR);
        return;
    }

    _mqttClient.setServer(MQTT_IP, MQTT_PORT);
    _mqttClient.setBufferSize(MQTT_MAX_MESSAGE_SIZE);
    _mqttClient.setCallback(BrokerSingleton::_mqttMessageReceived);

    int maxAttemps = 10;
    int iteration = 0;
    while (!_connected || iteration < maxAttemps)
    {
        iteration++;
        if (_mqttClient.connect(MQTT_CLIENT_ID))
        {
            Serial.println("Connexion au broker MQTT réussie.");
            _connected = true;
            _boardStateService.SetState(BoardState::OK);

            for (auto topic : Topics)
            {
                Subscribe(topic->c_str());
            }
            return;
        }
    }

    Serial.println("Échec de la connexion au broker MQTT.");
    _boardStateService.SetState(BoardState::ERROR);
}

void BrokerSingleton::_disconnect()
{
    if (_connected)
    {
        _boardStateService.SetState(BoardState::PENDING);
        _mqttClient.disconnect();
        _connected = false;
        _boardStateService.SetState(BoardState::OK);
        Serial.println("Déconnecté du broker MQTT.");
    }
}

void BrokerSingleton::_startAcquisition()
{
    if (_acquireMessages)
    {
        return;
    }
    else
    {
        _acquireMessages = true;
    }

    while (_acquireMessages)
    {
        // Appel à loop() pour gérer le MQTT
        if (!_mqttClient.loop())
        {
            // Si la connexion est perdue, tenter de reconnecter
            Serial.println("MQTT connection lost, trying to reconnect...");
            _connected = false;
            _connect(); // Tentative de reconnexion
        }

        // Pause légère pour éviter de saturer le CPU
        // vTaskDelay(10 / portTICK_PERIOD_MS);
    }
}

void BrokerSingleton::_stopAcquisition()
{
    _acquireMessages = false;
}

void BrokerSingleton::_mqttMessageReceived(char *topic, byte *payload, unsigned int length)
{
    String topicStr = String(topic);
    Serial.println("Received a message on topic :" + String(topic));

    String messagePayload;
    for (unsigned int i = 0; i < length; i++)
    {
        messagePayload += (char)payload[i];
    }

    BrokerSingleton *retreivedInstance = BrokerSingleton::GetInstance();
    bool handled = false;
    MqttResponse retreivedResponse;
    MqttRequest retreivedRequest;

    // Response handling
    try
    {
        Serial.println("Trying to cast into response ");
        retreivedResponse.FromString(messagePayload);
        Serial.println("Response cast result :" + retreivedResponse.ToString());

        String correspondingRequestId = retreivedResponse.Header.RequestIdentifier;

        Serial.println("Start trying to look for a corresponding request");

        int indexValue = -1;
        for (MqttPendingRequestPromise *pendingRequest : retreivedInstance->PendingRequestPromise)
        {
            indexValue++;
            Serial.println(pendingRequest->RequestIdentifier);
            Serial.println(correspondingRequestId);

            if (pendingRequest->RequestIdentifier == correspondingRequestId)
            {
                Serial.println("Request found, completionref.set_value");
                try
                {
                    pendingRequest->CompletionRef.set_value(retreivedResponse);
                    handled = true;
                    break;
                }
                catch (const std::exception &e)
                {
                    break;
                }
            }
        }

        // If response has been handled we can delete its corresponding item in queue
        if (indexValue >= 0 && handled)
        {
            retreivedInstance->PendingRequestPromise.erase(retreivedInstance->PendingRequestPromise.begin() + indexValue);
        }
    }
    catch (const std::exception &e)
    {
        handled = false;
    }

    // Request handling
    try
    {
        Serial.println("Trying to cast into request !!");

        retreivedRequest.FromString(messagePayload);

        Serial.println("Received a request !!");

        for (IMqttRequestHandler *handler : retreivedInstance->Handlers)
        {
            if (handler->IsAbleToHandle(retreivedRequest.JsonType))
            {
                Serial.println("Request handler found");

                handler->Handle(topicStr, retreivedRequest);
                return;
            }
        }
    }
    catch (const std::exception &e)
    {
        Serial.println("Unable to cast resuqest");
        Serial.println(e.what());
        String requestEmitter = String(DefaultIdentifiers::DEFAULT_SENDER_ID);
        String requestIdentifier = String(DefaultIdentifiers::DEFAULT_REQUEST_ID);

        retreivedInstance->SendErrorResponse(requestEmitter,
                                             requestIdentifier,
                                             topic,
                                             ExecutionResults::CONTRACT_ERROR,
                                             MqttBaseResponses::CONTRACT_ERROR);
        return;
    }

    // Error handling
    if (!handled)
    {
        String requestEmitter = String(DefaultIdentifiers::DEFAULT_SENDER_ID);
        String requestIdentifier = String(DefaultIdentifiers::DEFAULT_REQUEST_ID);

        if (retreivedRequest.Header.RequestEmitter.length() > 0 &&
            retreivedRequest.Header.RequestIdentifier.length() > 0)
        {
            requestEmitter = retreivedRequest.Header.RequestEmitter;
            requestIdentifier = retreivedRequest.Header.RequestIdentifier;
        }

        if (retreivedResponse.Header.RequestEmitter.length() > 0 &&
            retreivedResponse.Header.RequestIdentifier.length() > 0)
        {
            requestEmitter = retreivedResponse.Header.RequestEmitter;
            requestIdentifier = retreivedResponse.Header.RequestIdentifier;
        }

        retreivedInstance->SendErrorResponse(requestEmitter,
                                             requestIdentifier,
                                             topic);
    }
}
