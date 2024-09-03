namespace SmartWeather.Services.Mqtt.Dtos;

public class StationConfigRequest
{
    public int MacAddress { get; set; }
    public IEnumerable<int> ActivePins { get; set; } = null!;
}
