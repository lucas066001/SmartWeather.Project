namespace SmartWeather.Services.Mqtt.Dtos;

public class StationConfigResponse
{
    public record MeasurePointConfig
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
    }
    public record ComponentConfig
    {
        public int GpioPin { get; set; }
        public int DatabaseId { get; set; }
        public IEnumerable<MeasurePointConfig> MeasurePointsConfigs { get; set; } = null!;
    }
    public int StationDatabaseId { get; set; }
    public IEnumerable<ComponentConfig> ConfigComponents { get; set; }

    public StationConfigResponse(int stationId, IEnumerable<ComponentConfig> components)
    {
        StationDatabaseId = stationId;
        ConfigComponents = components;
    }
}
