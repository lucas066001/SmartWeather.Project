#include <Mqtt/MqttContractConstants.h>

namespace SmartWeather::Constants::Mqtt {

    const char* MqttBaseResponses::OK = "Request executed without errors";
    const char* MqttBaseResponses::ARGUMENT_ERROR = "Argument error, Unable to process request based on given parameters";
    const char* MqttBaseResponses::INTERNAL_ERROR = "Unknow error occured during request processing";
    const char* MqttBaseResponses::TIMEOUT_ERROR = "Timeout error occured due to processing time taking too long";
    const char* MqttBaseResponses::CONTRACT_ERROR = "The object you are sending is not supported";
    const char* MqttBaseResponses::DATABASE_ERROR = "Database error occured";
    const char* MqttBaseResponses::FORMAT_ERROR = "Runtime error : {0}";

    const char* DefaultIdentifiers::DEFAULT_REQUEST_ID = "UNKNOWN";
    const char* DefaultIdentifiers::DEFAULT_SENDER_ID = "UNKNOWN";

}