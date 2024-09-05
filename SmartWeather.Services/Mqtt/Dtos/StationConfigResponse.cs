namespace SmartWeather.Services.Mqtt.Dtos;

public class StationConfigResponse
{
    public record ComponentConfig
    {
        public required int GpioPin { get; set; }
        public required int ComponentDatabaseId { get; set; }
    }
    public int StationDatabaseId { get; set; }
    public string TopicLocation { get; set; }
    public IEnumerable<ComponentConfig> ConfigComponents { get; set; }

    public StationConfigResponse(int stationId, string topicLocation, IEnumerable<ComponentConfig> components)
    {
        StationDatabaseId = stationId;
        TopicLocation = topicLocation;
        ConfigComponents = components;
    }
}
