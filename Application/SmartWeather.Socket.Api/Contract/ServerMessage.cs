using static SmartWeather.Socket.Api.Contract.Constants;

namespace SmartWeather.Socket.Api.Contract;

public class ServerMessage
{
    public MessageStatus Status { get; set; }
    public required string OriginCall { get; set; }

}
