namespace SmartWeather.Api.Controllers.Station;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Stations;
using SmartWeather.Api.Controllers.Station.Dtos;
using SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Entities.Station;
using SmartWeather.Services.Authentication;
using SmartWeather.Entities.User;
using SmartWeather.Api.Helpers;
using SmartWeather.Entities.Common.Exceptions;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StationController : ControllerBase
{
    private readonly StationService _stationService;
    private readonly AuthenticationService _authenticationService;
    private readonly AccessManagerHelper _accessManagerHelper;

    public StationController(StationService stationService, AuthenticationService authenticationService, AccessManagerHelper accessManagerHelper)
    {
        _stationService = stationService;
        _authenticationService = authenticationService;
        _accessManagerHelper = accessManagerHelper;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<StationResponse>> Create(StationCreateRequest request)
    {
        ApiResponse<StationResponse> response;
        StationResponse formattedResponse;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            !(request.UserId > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        var createdStation = _stationService.AddNewStation(request.Name, request.MacAddress, request.Latitude, request.Longitude, request.UserId);

        if (createdStation.IsFailure)
        {
            response = ApiResponse<StationResponse>.Failure(createdStation.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = StationResponseConverter.ConvertStationToStationResponse(createdStation.Value);
        response = ApiResponse<StationResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

    [HttpPost(nameof(Claim))]
    public ActionResult<ApiResponse<StationResponse>> Claim(StationClaimRequest request)
    {
        ApiResponse<StationResponse> response;
        StationResponse formattedResponse;

        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(request.MacAddress) ||
            string.IsNullOrEmpty(token))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        var userId = _authenticationService.GetUserIdFromToken(token);

        if (userId == -1)
        {
            response = ApiResponse<StationResponse>.Failure();
            return Unauthorized(response);
        }

        var retreivedStation = _stationService.GetStationByMacAddress(request.MacAddress);

        if (retreivedStation.IsFailure)
        {
            response = ApiResponse<StationResponse>.Failure(retreivedStation.ErrorMessage);
            return BadRequest(response);
        }

        var updatedStation = _stationService.UpdateStation(retreivedStation.Value.Id,
                                        retreivedStation.Value.Name,
                                        retreivedStation.Value.MacAddress,
                                        retreivedStation.Value.Latitude,
                                        retreivedStation.Value.Longitude,
                                        userId);

        if (updatedStation.IsFailure)
        {
            response = ApiResponse<StationResponse>.Failure(updatedStation.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = StationResponseConverter.ConvertStationToStationResponse(updatedStation.Value);
        response = ApiResponse<StationResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<StationResponse>> Update(StationUpdateRequest request)
    {
        ApiResponse<StationResponse> response;
        StationResponse formattedResponse;

        if (!(request.Id > 0) ||
            string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Station>(this, request.Id, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<StationResponse>.Failure();
            return Unauthorized(response);
        }


        var updatedStation = _stationService.UpdateStation(request.Id, request.Name, request.MacAddress, request.Latitude, request.Longitude, request.UserId);

        if (updatedStation.IsFailure)
        {
            response = ApiResponse<StationResponse>.Failure(updatedStation.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = StationResponseConverter.ConvertStationToStationResponse(updatedStation.Value);
        response = ApiResponse<StationResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idStation)
    {
        ApiResponse<EmptyResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Station>(this, idStation, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<EmptyResponse>.Failure();
            return Unauthorized(response);
        }

        bool isStationDeleted = _stationService.DeleteStation(idStation);
        if (isStationDeleted)
        {
            response = ApiResponse<EmptyResponse>.Success(null);
            return Ok(response);
        }
        else
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.DATABASE_ERROR);
            return BadRequest(response);
        }
    }

    [HttpGet(nameof(GetFromUser))]
    public ActionResult<ApiResponse<StationListResponse>> GetFromUser(int userId)
    {
        ApiResponse<StationListResponse> response;

        if (!(userId > 0))
        {
            return BadRequest(ApiResponse<StationListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<User>(this, userId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<StationListResponse>.Failure();
            return Unauthorized(response);
        }

        var stationList = _stationService.GetFromUser(userId);


        if (stationList.IsFailure)
        {
            response = ApiResponse<StationListResponse>.Failure(stationList.ErrorMessage);
            return BadRequest(response);
        }

        StationListResponse formattedResponse = StationListResponseConverter.ConvertStationListToStationListResponse(stationList.Value);
        response = ApiResponse<StationListResponse>.Success(formattedResponse);
        
        return Ok(response);
    }


    [HttpGet(nameof(GetById))]
    public ActionResult<ApiResponse<StationResponse>> GetById(int idStation)
    {
        ApiResponse<StationResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Station>(this, idStation, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<StationResponse>.Failure();
            return Unauthorized(response);
        }


        var stationRetreived = _stationService.GetStationById(idStation);

        if (stationRetreived.IsFailure)
        {
            response = ApiResponse<StationResponse>.Failure(stationRetreived.ErrorMessage);
            return BadRequest(response);
        }

        response = ApiResponse<StationResponse>.Success(StationResponseConverter.ConvertStationToStationResponse(stationRetreived.Value));
        
        return Ok(response);
    }

    [HttpGet(nameof(GetAll))]
    public ActionResult<ApiResponse<StationListResponse>> GetAll(bool includeComponents, bool includeMeasurePoints)
    {
        ApiResponse<StationListResponse> response;
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");


        var userRole = _authenticationService.GetUserRoleFromToken(token);

        if (userRole == Role.Unauthorized || !RoleAccess.ADMINISTRATORS.Contains(userRole))
        {
            response = ApiResponse<StationListResponse>.Failure(ExceptionsBaseMessages.SECURITY);
            return Unauthorized(response);
        }

        var stationsRetreived = _stationService.GetAll(includeComponents, includeMeasurePoints);

        if (stationsRetreived.IsFailure)
        {
            response = ApiResponse<StationListResponse>.Failure(stationsRetreived.ErrorMessage);
            return Unauthorized(response);
        }

        response = ApiResponse<StationListResponse>.Success(StationListResponseConverter.ConvertStationListToStationListResponse(stationsRetreived.Value));
        
        return Ok(response);
    }

    [HttpGet(nameof(GetStationsMeasurePoints))]
    public ActionResult<ApiResponse<List<StationMeasurePointsResponse>>> GetStationsMeasurePoints()
    {
        ApiResponse<List<StationMeasurePointsResponse>> response;
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        var userRole = _authenticationService.GetUserRoleFromToken(token);

        if (userRole == Role.Unauthorized || !RoleAccess.ADMINISTRATORS.Contains(userRole))
        {
            response = ApiResponse<List<StationMeasurePointsResponse>>.Failure(ExceptionsBaseMessages.SECURITY);
            return Unauthorized(response);
        }

        var stationsRetreived = _stationService.GetAll(true, true);

        if (stationsRetreived.IsFailure)
        {
            response = ApiResponse<List<StationMeasurePointsResponse>>.Failure(stationsRetreived.ErrorMessage);
            return BadRequest(response);
        }

        List<StationMeasurePointsResponse> result = new List<StationMeasurePointsResponse>();
        foreach (Station station in stationsRetreived.Value)
        {
            result.Add(StationMeasurePointsResponseConverter.ConvertStationResponseToStationMeasurePointResponse(station));
        }
        response = ApiResponse<List<StationMeasurePointsResponse>>.Success(result);
            
        return Ok(response);
    }
}
