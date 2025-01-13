namespace SmartWeather.Api.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Entities.ActivationPlan;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Services.ActivationPlan;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.Components;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Stations;

public class AccessManagerHelper
{
    private readonly AuthenticationService _authenticationService;
    private readonly StationService _stationService;
    private readonly ComponentService _componentService;
    private readonly MeasurePointService _measurePointService;
    private readonly ActivationPlanService _activationPlanService;

    public AccessManagerHelper(AuthenticationService authenticationService,
                               StationService stationService,
                               ComponentService componentService,
                               MeasurePointService measurePointService,
                               ActivationPlanService activationPlanService)
    {
        _authenticationService = authenticationService;
        _stationService = stationService;
        _componentService = componentService;
        _measurePointService = measurePointService;
        _activationPlanService = activationPlanService;
    }

    /// <summary>
    /// Validate if User has access to desired entity.
    /// </summary>
    /// <typeparam name="T">Entity type to check access to.</typeparam>
    /// <param name="controllerContext">Controller context to get token from. (Mainly 'this' if used in controller class)</param>
    /// <param name="entityId">Entity unique Id.</param>
    /// <param name="grantedRole">Optional list of roles that has access, even if not owner.</param>
    /// <returns>Bool indicating if current user has access to ressource.</returns>
    public bool ValidateUserAccess<T>(ControllerBase controllerContext, int entityId, List<Role>? grantedRole = null)
                                         where T : class
    {
        try
        {
            var token = controllerContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var userId = _authenticationService.GetUserIdFromToken(token);
            var userRole = _authenticationService.GetUserRoleFromToken(token);

            if (userId == -1 || userRole == Role.Unauthorized)
            {
                return false;
            }

            bool isOwner = false;
            if (typeof(T) == typeof(Station))
            {
                isOwner = _stationService.IsOwnerOfStation(userId, entityId);
            }
            else if(typeof(T) == typeof(Component))
            {
                isOwner = _componentService.IsOwnerOfComponent(userId, entityId);
            }
            else if (typeof(T) == typeof(MeasurePoint))
            {
                isOwner = _measurePointService.IsOwnerOfMeasurePoint(userId, entityId);
            }
            else if (typeof(T) == typeof(User))
            {
                isOwner = entityId == userId;
            }
            else if (typeof(T) == typeof(ActivationPlan))
            {
                isOwner = _activationPlanService.IsOwnerOfActivationPlan(userId, entityId);
            }

            if (isOwner || (grantedRole != null && grantedRole.Contains(userRole)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex) when (ex is SecurityTokenException)
        {
            return false;
        }
    }
}
