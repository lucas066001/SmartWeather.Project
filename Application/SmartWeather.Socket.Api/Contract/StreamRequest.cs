namespace SmartWeather.Socket.Api.Contract;

public class StreamRequest
{
    public required string Token { get; set; }
    public int TargetId { get; set; }
}
