using Elastic.Clients.Elasticsearch.Core.TermVectors;
using Microsoft.AspNetCore.SignalR;
using SmartWeather.Entities.User;
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
        Role userRole = _authenticationService.GetUserRoleFromToken(request.Token);

        if (RoleAccess.GLOBAL_READING_ACCESS.Contains(userRole) || 
            (userId > 0 && _measurePointService.IsOwnerOfMeasurePoint(userId, request.TargetId)))
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, 
                                          string.Format(Constants.GROUP_NAMING_FORMAT,
                                                        Constants.STREAM_SINGLE_MP,
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
                                                         Constants.STREAM_SINGLE_MP,
                                                         request.TargetId.ToString()));
    }

    public Task InitiateMonitoring(MonitoringRequest request)
    {
        Role userRole = _authenticationService.GetUserRoleFromToken(request.Token);

        if (RoleAccess.GLOBAL_READING_ACCESS.Contains(userRole))
        {
            return Groups.AddToGroupAsync(Context.ConnectionId,
                                          Constants.STREAM_MONITORING);
        }
        else
        {
            throw new TaskCanceledException("You are not allowed to watch this data");
        }
    }

    public Task CloseMonitoring()
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId,
                                           Constants.STREAM_MONITORING);
    }
}
