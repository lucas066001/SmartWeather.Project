namespace SmartWeather.Api.Controllers.Station;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Stations;
using SmartWeather.Api.Controllers.Station.Dtos;
using SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Entities.Station;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StationController : ControllerBase
{
    private readonly StationService _stationService;

    public StationController(StationService stationService)
    {
        _stationService = stationService;
    }

    [Authorize]
    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<StationResponse>> Create(StationCreateRequest request)
    {
        ApiResponse<StationResponse> response;
        StationResponse formattedResponse;

        if (String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.TopicLocation) ||
            !(request.UserId > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            Entities.Station.Station createdStation = _stationService.AddNewStation(request.Name, request.Latitude, request.Longitude, request.TopicLocation, request.UserId);
            formattedResponse = StationResponseConverter.ConvertStationToStationResponse(createdStation);
            response = ApiResponse<StationResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<StationResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<StationResponse>> Update(StationUpdateRequest request)
    {
        ApiResponse<StationResponse> response;
        StationResponse formattedResponse;

        if (!(request.Id > 0) ||
            String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.TopicLocation) ||
            !(request.Id > 0))
        {
            return BadRequest(ApiResponse<StationResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            Entities.Station.Station updatedStation = _stationService.UpdateStation(request.Id, request.Name, request.Latitude, request.Longitude, request.TopicLocation, request.UserId);
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

    [Authorize]
    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idStation)
    {
        // Later will need to restrict this to admin or current token user privilege
        ApiResponse<EmptyResponse> response;

        if (!(idStation > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
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

    [Authorize]
    [HttpGet(nameof(GetFromUser))]
    public ActionResult<ApiResponse<StationListResponse>> GetFromUser(int userId)
    {
        // Need to implement
        ApiResponse<StationListResponse> response;

        if (!(userId > 0))
        {
            return BadRequest(ApiResponse<StationListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
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
        catch (Exception ex)
        {
            response = ApiResponse<StationListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }


    //[Authorize]
    //[HttpGet(nameof(GetById))]
    //public ActionResult<ApiResponse<UserResponse>> GetById(int idUser)
    //{
    //    // Later will need to restrict this to admin or current token user privilege
    //    ApiResponse<UserResponse> response;

    //    if (!(idUser > 0))
    //    {
    //        return BadRequest(ApiResponse<UserResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
    //    }

    //    try
    //    {
    //        SmartWeather.Entities.User.User userRetreived = _userService.GetUserById(idUser);
    //        if (userRetreived != null)
    //        {
    //            response = ApiResponse<UserResponse>.Success(UserResponseConverter.ConvertUserToUserResponse(userRetreived));
    //        }
    //        else
    //        {
    //            response = ApiResponse<UserResponse>.Failure(BaseResponses.INTERNAL_ERROR);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        response = ApiResponse<UserResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
    //        return BadRequest(response);
    //    }

    //    return Ok(response);
    //}

    //[Authorize]
    //[HttpGet(nameof(GetAll))]
    //public ActionResult<ApiResponse<UserListResponse>> GetAll()
    //{
    //    // Later will need to restrict this to admin or current token user privilege
    //    ApiResponse<UserListResponse> response;

    //    try
    //    {
    //        IEnumerable<SmartWeather.Entities.User.User> usersRetreived = _userService.GetUserList(null);
    //        response = ApiResponse<UserListResponse>.Success(UserResponseConverter.ConvertUserListToUserListResponse(usersRetreived));
    //    }
    //    catch (Exception ex)
    //    {
    //        response = ApiResponse<UserListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
    //        return BadRequest(response);
    //    }

    //    return Ok(response);
    //}

}
