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
using SmartWeather.Entities.User;
using SmartWeather.Entities.Component;
using SmartWeather.Api.Helpers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeasurePointController : Controller
{
    private readonly MeasurePointService _measurePointService;
    private readonly AuthenticationService _authenticationService;
    private readonly ComponentService _componentService;
    private readonly AccessManagerHelper _accessManagerHelper;

    public MeasurePointController(MeasurePointService measurePointService, AuthenticationService authenticationService, ComponentService componentService, AccessManagerHelper accessManagerHelper)
    {
        _measurePointService = measurePointService;
        _authenticationService = authenticationService;
        _componentService = componentService;
        _accessManagerHelper = accessManagerHelper;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<MeasurePointResponse>> Create(MeasurePointCreateRequest request)
    {
        ApiResponse<MeasurePointResponse> response;
        MeasurePointResponse formattedResponse;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Color) ||
            !(request.ComponentId > 0) ||
            !(request.LocalId > 0) ||
            !(request.Unit >= 0))
        {
            return BadRequest(ApiResponse<MeasurePointResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, request.ComponentId, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<MeasurePointResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var createdComponentData = _measurePointService.AddNewMeasurePoint(request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);

        if (createdComponentData.IsFailure)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(createdComponentData.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(createdComponentData.Value);
        response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
        
        return Ok(response);
        
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idMeasurePoint)
    {
        ApiResponse<EmptyResponse> response;

        if (!(idMeasurePoint > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<MeasurePoint>(this, idMeasurePoint, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        bool isMeasurePointDeleted = _measurePointService.DeleteMeasurePoint(idMeasurePoint);
        if (isMeasurePointDeleted)
        {
            response = ApiResponse<EmptyResponse>.Success(new EmptyResponse());
            return Ok(response);
        }
        else
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.DATABASE_ERROR);
            return BadRequest(response);
        }
    }

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<MeasurePointResponse>> Update(MeasurePointUpdateRequest request)
    {
        ApiResponse<MeasurePointResponse> response;
        MeasurePointResponse formattedResponse;

        if (!(request.Id > 0) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Color) ||
            !(request.ComponentId > 0) ||
            !(request.LocalId > 0) || 
            !(request.Unit >= 0))
        {
            return BadRequest(ApiResponse<MeasurePointResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<MeasurePoint>(this, request.Id, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<MeasurePointResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var updatedMeasurePoint = _measurePointService.UpdateMeasurePoint(request.Id, request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);

        if (updatedMeasurePoint.IsFailure)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(updatedMeasurePoint.ErrorMessage);
            return BadRequest(response);
        }
        
        formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(updatedMeasurePoint.Value);
        response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

    [HttpGet(nameof(GetFromComponent))]
    public ActionResult<ApiResponse<MeasurePointListResponse>> GetFromComponent(int componentId)
    {
        ApiResponse<MeasurePointListResponse> response;

        if (!(componentId > 0))
        {
            return BadRequest(ApiResponse<MeasurePointListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, componentId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<MeasurePointListResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var measurePointList = _measurePointService.GetFromComponent(componentId);

        if (measurePointList.IsFailure)
        {
            response = ApiResponse<MeasurePointListResponse>.Failure(measurePointList.ErrorMessage);
            return BadRequest(response);
        }

        MeasurePointListResponse formattedResponse = MeasurePointConverter.ConvertMeasurePointListToMeasurePointListResponse(measurePointList.Value);
        response = ApiResponse<MeasurePointListResponse>.Success(formattedResponse);
        
        return Ok(response);
    }
}
