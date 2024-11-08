using Microsoft.AspNetCore.SignalR;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Socket.Api.Contract;

namespace SmartWeather.Socket.Api.Hubs.MeasurePoint;

public class MeasurePointHub : Hub
{
    private readonly AuthenticationService _authenticationService;
    private readonly MeasurePointService _measurePointService;

    public MeasurePointHub(AuthenticationService authenticationService, MeasurePointService measurePointService)
    {
        _authenticationService = authenticationService;
        _measurePointService = measurePointService;
    }
    public Task InitiateStream(StreamRequest request)
    {
        var userId = _authenticationService.GetUserIdFromToken(request.Token);

        if (userId > 0 && _measurePointService.IsOwnerOfMeasurePoint(userId, request.TargetId))
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, 
                                          string.Format(Constants.GROUP_NAMING_FORMAT,
                                                        nameof(MeasurePointHub),
                                                        request.TargetId.ToString())
                                          );
        }
        else
        {
            throw new TaskCanceledException("You are not allowed to watch this data");
        }
    }

    public Task CloseStream(StreamRequest request)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId,
                                           string.Format(Constants.GROUP_NAMING_FORMAT,
                                                         nameof(MeasurePointHub),
                                                         request.TargetId.ToString()));
    }
}
