namespace SmartWeather.StationMocker.Constants;

public class Configuration
{
    public static string API_URL = "API_URL";
    public static string STATION_NUMBER = "STATION_NB";
    public static string COMPONENT_NUMBER = "COMPONENT_NB";
    public static string ERROR_RATE = "MAX_ERROR_RATE";
    public static string DATA_FREQUENCY = "DATA_FREQ";
    public static int MAX_TREND_STRENGH = 10;
    public static int MAX_TREND_LENGHT = 80;
    public enum MeasureType
    {
        UNSET,
        TEMP,
        HUMIDITY,
        UV
    }
}