namespace SmartWeather.Services.Mqtt.Dtos;

public class StationConfigResponse
{
    public record ComponentConfig
    {
        public required int GpioPin { get; set; }
        public required int ComponentDatabaseId { get; set; }
    }
    public int StationDatabaseId { get; set; }
    public IEnumerable<ComponentConfig> ConfigComponents { get; set; }

    public StationConfigResponse(int stationId, IEnumerable<ComponentConfig> components)
    {
        StationDatabaseId = stationId;
        ConfigComponents = components;
    }
}
