namespace SmartWeather.Api.Contract;

public enum Status
{
    OK,
    INTERNAL_ERROR,
    NO_CONTENT,
    TIMEOUT_ERROR,
    DATABASE_ERROR,
    VALIDATION_ERROR,
    AUTHORIZATION_ERROR,
}

public class BaseResponses
{
    public const string OK = "Request executed without errors";
    public const string ARGUMENT_ERROR = "Argument error, Unable to process request based on given parameters";
    public const string INTERNAL_ERROR = "Unknow error occured during request processing";
    public const string TIMEOUT_ERROR = "Timeout error occured due to processing time taking too long";
    public const string DATABASE_ERROR = "Database error occured";
    public const string AUTHORIZATION_ERROR = "Unable to give you access to the request ressource";
    public const string VALIDATION_ERROR = "Unable to create entity based on given informations";
    public const string NO_CONTENT = "No data has been retreived";
    public const string FORMAT_ERROR = "Runtime error : {0}";
}
