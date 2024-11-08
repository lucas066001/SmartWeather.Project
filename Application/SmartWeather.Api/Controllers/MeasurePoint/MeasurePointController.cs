namespace SmartWeather.Api.Controllers.MeasurePoint;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Api.Controllers.MeasurePoint.Dtos;
using SmartWeather.Api.Controllers.MeasurePoint.Dtos.Converters;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.Components;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Entities.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeasurePointController : Controller
{
    private readonly MeasurePointService _measurePointService;
    private readonly AuthenticationService _authenticationService;
    private readonly ComponentService _componentService;

    public MeasurePointController(MeasurePointService measurePointService, AuthenticationService authenticationService, ComponentService componentService)
    {
        _measurePointService = measurePointService;
        _authenticationService = authenticationService;
        _componentService = componentService;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<MeasurePointResponse>> Create(MeasurePointCreateRequest request)
    {
        ApiResponse<MeasurePointResponse> response;
        MeasurePointResponse formattedResponse;

        if (String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.Color) ||
            !(request.ComponentId > 0) ||
            !(request.LocalId > 0) ||
            !(request.Unit >= 0))
        {
            return BadRequest(ApiResponse<MeasurePointResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            MeasurePoint createdComponentData = _measurePointService.AddNewMeasurePoint(request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);
            formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(createdComponentData);
            response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idMeasurePoint)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        ApiResponse<EmptyResponse> response;

        if (!(idMeasurePoint > 0) ||
            !string.IsNullOrEmpty(token))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!_measurePointService.IsOwnerOfMeasurePoint(userId, idMeasurePoint) &&
                !RoleAccess.ADMINISTRATORS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to delete this element");
            }

            bool isMeasurePointDeleted = _measurePointService.DeleteMeasurePoint(idMeasurePoint);
            if (isMeasurePointDeleted)
            {
                response = ApiResponse<EmptyResponse>.Success(null);
            }
            else
            {
                response = ApiResponse<EmptyResponse>.Failure(BaseResponses.INTERNAL_ERROR);
            }

        }
        catch (UnauthorizedAccessException ex)
        {
            response = ApiResponse<EmptyResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<EmptyResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<MeasurePointResponse>> Update(MeasurePointUpdateRequest request)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        ApiResponse<MeasurePointResponse> response;
        MeasurePointResponse formattedResponse;

        if (!(request.Id > 0) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Color) ||
            !string.IsNullOrEmpty(token) ||
            !(request.ComponentId > 0) ||
            !(request.LocalId > 0) || 
            !(request.Unit >= 0))
        {
            return BadRequest(ApiResponse<MeasurePointResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!_measurePointService.IsOwnerOfMeasurePoint(userId, request.Id) &&
                !RoleAccess.GLOBAL_READING_ACCESS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to modify this element");
            }

            MeasurePoint updatedMeasurePoint = _measurePointService.UpdateMeasurePoint(request.Id, request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);
            formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(updatedMeasurePoint);
            response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);

        }
        catch (SecurityTokenException ex)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(nameof(GetFromComponent))]
    public ActionResult<ApiResponse<MeasurePointListResponse>> GetFromComponent(int componentId)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        ApiResponse<MeasurePointListResponse> response;

        if (!(componentId > 0) ||
            !string.IsNullOrEmpty(token))
        {
            return BadRequest(ApiResponse<MeasurePointListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!_componentService.IsOwnerOfComponent(userId, componentId) &&
                !RoleAccess.GLOBAL_READING_ACCESS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to get this information");
            }

            IEnumerable<MeasurePoint> measurePointList = _measurePointService.GetFromComponent(componentId);
            if (measurePointList.Any())
            {
                MeasurePointListResponse formattedResponse = MeasurePointConverter.ConvertMeasurePointListToMeasurePointListResponse(measurePointList);
                response = ApiResponse<MeasurePointListResponse>.Success(formattedResponse);
            }
            else
            {
                response = ApiResponse<MeasurePointListResponse>.Success(null);
            }

        }
        catch (SecurityTokenException ex)
        {
            response = ApiResponse<MeasurePointListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            response = ApiResponse<MeasurePointListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasurePointListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

}
