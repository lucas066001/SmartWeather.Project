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
using SmartWeather.Services.Components;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Api.Controllers.MeasurePoint.Dtos;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StationController : ControllerBase
{
    private readonly StationService _stationService;
    private readonly AuthenticationService _authenticationService;

    public StationController(StationService stationService, AuthenticationService authenticationService)
    {
        _stationService = stationService;
        _authenticationService = authenticationService;
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

        try
        {
            Station createdStation = _stationService.AddNewStation(request.Name, request.MacAddress, request.Latitude, request.Longitude, request.UserId);
            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(createdStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

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

        Console.WriteLine(token);

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);

            if (userId <= 0)
            {
                throw new Exception("Unable to retreive user info from token");
            }

            Station? retreivedStation = _stationService.GetStationByMacAddress(request.MacAddress);
            if (retreivedStation == null)
            {
                throw new Exception("MacAdress doesn't correspond to any registered station");
            }

            retreivedStation = _stationService.UpdateStation(retreivedStation.Id,
                                          retreivedStation.Name,
                                          retreivedStation.MacAddress,
                                          retreivedStation.Latitude,
                                          retreivedStation.Longitude,
                                          userId);

            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(retreivedStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<StationResponse>> Update(StationUpdateRequest request)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        ApiResponse<StationResponse> response;
        StationResponse formattedResponse;

        if (!(request.Id > 0) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            !(request.Id > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!_stationService.IsOwnerOfStation(userId, request.Id) &&
                !RoleAccess.ADMINISTRATORS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to update this information");
            }

            Station updatedStation = _stationService.UpdateStation(request.Id, request.Name, request.MacAddress, request.Latitude, request.Longitude, request.UserId);
            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(updatedStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idStation)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        ApiResponse<EmptyResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!_stationService.IsOwnerOfStation(userId, idStation) &&
                !RoleAccess.ADMINISTRATORS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to delete this information");
            }

            bool isStationDeleted = _stationService.DeleteStation(idStation);
            if (isStationDeleted)
            {
                response = ApiResponse<EmptyResponse>.Success(null);
            }
            else
            {
                response = ApiResponse<EmptyResponse>.Failure(BaseResponses.INTERNAL_ERROR);
            }

        }
        catch (Exception ex)
        {
            response = ApiResponse<EmptyResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(nameof(GetFromUser))]
    public ActionResult<ApiResponse<StationListResponse>> GetFromUser(int userId)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        ApiResponse<StationListResponse> response;

        if (!(userId > 0))
        {
            return BadRequest(ApiResponse<StationListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userIdToken = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (userIdToken != userId &&
                !RoleAccess.GLOBAL_READING_ACCESS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to get this information");
            }

            IEnumerable<Station> stationList = _stationService.GetFromUser(userId);
            StationListResponse formattedResponse = StationListResponseConverter.ConvertStationListToStationListResponse(stationList);
            if (stationList.Any())
            {
                response = ApiResponse<StationListResponse>.Success(formattedResponse);
            }
            else
            {
                response = ApiResponse<StationListResponse>.Failure(BaseResponses.INTERNAL_ERROR);
            }

        }
        catch (SecurityTokenException ex)
        {
            response = ApiResponse<StationListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            response = ApiResponse<StationListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }


    [HttpGet(nameof(GetById))]
    public ActionResult<ApiResponse<StationResponse>> GetById(int idStation)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        
        ApiResponse<StationResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!_stationService.IsOwnerOfStation(userId, idStation) &&
                !RoleAccess.GLOBAL_READING_ACCESS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to get this information");
            }

            Station stationRetreived = _stationService.GetStationById(idStation);
            if (stationRetreived != null)
            {
                response = ApiResponse<StationResponse>.Success(StationResponseConverter.ConvertStationToStationResponse(stationRetreived));
            }
            else
            {
                response = ApiResponse<StationResponse>.Failure(BaseResponses.INTERNAL_ERROR);
            }

        }
        catch (SecurityTokenException ex)
        {
            response = ApiResponse<StationResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            response = ApiResponse<StationResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(nameof(GetAll))]
    public ActionResult<ApiResponse<StationListResponse>> GetAll(bool includeComponents, bool includeMeasurePoints)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        
        ApiResponse<StationListResponse> response;

        try
        {
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!RoleAccess.ADMINISTRATORS.Contains(userRole))
            {
                throw new UnauthorizedAccessException("Your are not allowed to get this information");
            }

            IEnumerable<Station> stationsRetreived = _stationService.GetAll(includeComponents, includeMeasurePoints);
            response = ApiResponse<StationListResponse>.Success(StationListResponseConverter.ConvertStationListToStationListResponse(stationsRetreived));
        
        }
        catch (SecurityTokenException ex)
        {
            response = ApiResponse<StationListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            response = ApiResponse<StationListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return Unauthorized(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationListResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

}
