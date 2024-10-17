namespace SmartWeather.Services.Mqtt.Dtos;

public class StationConfigRequest
{
    public record MeasurePointConfig
    {
        public int LocalId { get; set; }
        public string DefaultName { get; set; } = null!;
        public int Unit { get; set; }
    }

    public record PinConfig
    {
        public int GpioPin { get; set; }
        public int ComponentType { get; set; }
        public string DefaultName { get; set; } = null!;
        public IEnumerable<MeasurePointConfig> MeasurePoints { get; set; } = null!;
    }
    public required string MacAddress { get; set; }
    public IEnumerable<PinConfig> ComponentsConfigs { get; set; } = null!;
}
