namespace SmartWeather.Services.Mqtt.Contract;

public enum Status
{
    INTERNAL_ERROR,
    TIMEOUT_ERROR,
    DATABASE_ERROR,
    PARSE_ERROR,
    CONTRACT_ERROR,
    OK
}

public class BaseResponses
{
    public const string OK = "Request executed without errors";
    public const string ARGUMENT_ERROR = "Argument error, Unable to process request based on given parameters";
    public const string INTERNAL_ERROR = "Unknow error occured during request processing";
    public const string TIMEOUT_ERROR = "Timeout error occured due to processing time taking too long";
    public const string DATABASE_ERROR = "Database error occured";
    public const string FORMAT_ERROR = "Runtime error : {0}";
}

public enum ObjectTypes
{
    UNKNOWN,
    CONFIG_REQUEST,
    CONFIG_RESPONSE,
    SENSOR_SAVING_REQUEST,
    SENSOR_SAVING_RESPONSE,
    ACTUATOR_REQUEST,
    ACTUATOR_RESPONSE
}

public class DefaultIdentifiers
{
    public const string DEFAULT_REQUEST_ID = "UNKNOWN";
    public const string DEFAULT_SENDER_ID = "UNKNOWN";
}
