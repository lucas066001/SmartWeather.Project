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
    public static string STREAM_SINGLE_MP = "mpstream";
    public static string STREAM_MONITORING = "mpmonitoring";
}
