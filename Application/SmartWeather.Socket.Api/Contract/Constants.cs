namespace SmartWeather.Socket.Api.Contract;

public static class Constants
{
    public enum MessageStatus
    {
        Error,
        Success,
        Pending
    }

    public static string GROUP_NAMING_FORMAT = "{0}_{1}";
}
