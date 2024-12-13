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
using SmartWeather.Entities.Common.Exceptions;

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

        try
        {
            MeasurePoint createdComponentData = _measurePointService.AddNewMeasurePoint(request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);
            formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(createdComponentData);
            response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(BaseResponses.VALIDATION_ERROR, Status.VALIDATION_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<MeasurePointResponse>.Failure();
            return BadRequest(response);
        }
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

        try
        {
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
        catch
        {
            response = ApiResponse<EmptyResponse>.Failure();
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

        try
        {
            MeasurePoint updatedMeasurePoint = _measurePointService.UpdateMeasurePoint(request.Id, request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);
            formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(updatedMeasurePoint);
            response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntitySavingException || 
                                   ex is EntityCreationException) 
        {
            response = ApiResponse<MeasurePointResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<MeasurePointResponse>.Failure();
            return BadRequest(response);
        }
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

        try
        {
            IEnumerable<MeasurePoint> measurePointList = _measurePointService.GetFromComponent(componentId);
            MeasurePointListResponse formattedResponse = MeasurePointConverter.ConvertMeasurePointListToMeasurePointListResponse(measurePointList);
            response = ApiResponse<MeasurePointListResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<MeasurePointListResponse>.NoContent();
            return Ok(response);
        }
        catch
        {
            response = ApiResponse<MeasurePointListResponse>.Failure();
            return BadRequest(response);
        }
    }
}
