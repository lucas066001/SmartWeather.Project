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
using Microsoft.IdentityModel.Tokens;
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

        try
        {
            Station createdStation = _stationService.AddNewStation(request.Name, request.MacAddress, request.Latitude, request.Longitude, request.UserId);
            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(createdStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.VALIDATION_ERROR, Status.VALIDATION_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<StationResponse>.Failure();
            return BadRequest(response);
        }
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

        try
        {
            int userId = _authenticationService.GetUserIdFromToken(token);

            Station retreivedStation = _stationService.GetStationByMacAddress(request.MacAddress);

            retreivedStation = _stationService.UpdateStation(retreivedStation.Id,
                                          retreivedStation.Name,
                                          retreivedStation.MacAddress,
                                          retreivedStation.Latitude,
                                          retreivedStation.Longitude,
                                          userId);

            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(retreivedStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is SecurityTokenException)
        {
            response = ApiResponse<StationResponse>.Failure();
            return Unauthorized(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.INTERNAL_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.VALIDATION_ERROR, Status.VALIDATION_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<StationResponse>.Failure();
            return BadRequest(response);
        }
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

        if (_accessManagerHelper.ValidateUserAccess<Station>(this, request.Id, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<StationResponse>.Failure();
            return Unauthorized(response);
        }

        try
        {
            Station updatedStation = _stationService.UpdateStation(request.Id, request.Name, request.MacAddress, request.Latitude, request.Longitude, request.UserId);
            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(updatedStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.VALIDATION_ERROR, Status.VALIDATION_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<StationResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<StationResponse>.Failure();
            return BadRequest(response);
        }
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idStation)
    {
        ApiResponse<EmptyResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (_accessManagerHelper.ValidateUserAccess<Station>(this, idStation, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<EmptyResponse>.Failure();
            return Unauthorized(response);
        }

        try
        {
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
        catch
        {
            response = ApiResponse<EmptyResponse>.Failure();
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

        if (_accessManagerHelper.ValidateUserAccess<User>(this, userId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<StationListResponse>.Failure();
            return Unauthorized(response);
        }

        try
        {
            IEnumerable<Station> stationList = _stationService.GetFromUser(userId);
            StationListResponse formattedResponse = StationListResponseConverter.ConvertStationListToStationListResponse(stationList);
            response = ApiResponse<StationListResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<StationListResponse>.NoContent();
            return Ok(response);
        }
        catch 
        {
            response = ApiResponse<StationListResponse>.Failure();
            return BadRequest(response);
        }
    }


    [HttpGet(nameof(GetById))]
    public ActionResult<ApiResponse<StationResponse>> GetById(int idStation)
    {
        ApiResponse<StationResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (_accessManagerHelper.ValidateUserAccess<Station>(this, idStation, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<StationResponse>.Failure();
            return Unauthorized(response);
        }

        try
        {
            Station stationRetreived = _stationService.GetStationById(idStation);
            response = ApiResponse<StationResponse>.Success(StationResponseConverter.ConvertStationToStationResponse(stationRetreived));
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<StationResponse>.NoContent();
            return Ok(response);
        }
        catch
        {
            response = ApiResponse<StationResponse>.Failure();
            return BadRequest(response);
        }
    }

    [HttpGet(nameof(GetAll))]
    public ActionResult<ApiResponse<StationListResponse>> GetAll(bool includeComponents, bool includeMeasurePoints)
    {
        ApiResponse<StationListResponse> response;
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        try
        {
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!RoleAccess.ADMINISTRATORS.Contains(userRole))
            {
                throw new UnauthorizedAccessException();
            }
        }
        catch(Exception ex) when (ex is SecurityTokenException ||
                                  ex is UnauthorizedAccessException)
        {
            response = ApiResponse<StationListResponse>.Failure();
            return Unauthorized(response);
        }

        try
        {
            IEnumerable<Station> stationsRetreived = _stationService.GetAll(includeComponents, includeMeasurePoints);
            response = ApiResponse<StationListResponse>.Success(StationListResponseConverter.ConvertStationListToStationListResponse(stationsRetreived));
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<StationListResponse>.NoContent();
            return Ok(response);
        }
        catch 
        {
            response = ApiResponse<StationListResponse>.Failure();
            return BadRequest(response);
        }
    }

}
