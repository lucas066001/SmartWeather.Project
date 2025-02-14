namespace SmartWeather.Api.Controllers.Component;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Components;
using SmartWeather.Api.Controllers.Component.Dtos;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Api.Controllers.Component.Dtos.Converters;
using SmartWeather.Services.Mqtt;
using SmartWeather.Entities.User;
using SmartWeather.Api.Helpers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ComponentController : ControllerBase
{
    private readonly ComponentService _componentService;
    private readonly MqttService _mqttService;
    private readonly AccessManagerHelper _accessManagerHelper;

    public ComponentController(ComponentService componentService,
                                MqttService mqttService,
                                AccessManagerHelper accessManagerHelper)
    {
        _mqttService = mqttService;
        _componentService = componentService;
        _accessManagerHelper = accessManagerHelper;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<ComponentResponse>> Create(ComponentCreateRequest request)
    {
        ApiResponse<ComponentResponse> response;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Color) ||
            !(request.GpioPin >= 0) ||
            !(request.StationId > 0))
        {
            return BadRequest(ApiResponse<ComponentResponse>.Failure(BaseResponses.ARGUMENT_ERROR, 
                                                                     Status.VALIDATION_ERROR));
        }

        if(!_accessManagerHelper.ValidateUserAccess<Station>(this, request.StationId, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<ComponentResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var createdComponent = _componentService.AddNewComponent(request.Name, request.Color, request.Type, request.StationId, request.GpioPin);

        if (createdComponent.IsFailure)
        {
            response = ApiResponse<ComponentResponse>.Failure(createdComponent.ErrorMessage);
            return BadRequest(response);
        }
        
        var formattedResponse = ComponentResponseConverter.ConvertComponentToComponentResponse(createdComponent.Value);
        response = ApiResponse<ComponentResponse>.Success(formattedResponse);

        return Ok(response);

    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idComponent)
    {
        ApiResponse<EmptyResponse> response;

        if (idComponent <= 0)
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR,
                                                                 Status.VALIDATION_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, idComponent, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        bool isUserDeleted = _componentService.DeleteComponent(idComponent);
        if (isUserDeleted)
        {
            response = ApiResponse<EmptyResponse>.Success(null);
            return Ok(response);
        }
        else
        {
            response = ApiResponse<EmptyResponse>.Failure();
            return BadRequest(response);
        }
    }

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<ComponentResponse>> Update(ComponentUpdateRequest request)
    {
        ApiResponse<ComponentResponse> response;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Color) ||
            !(request.GpioPin >= 0) ||
            !(request.StationId > 0) ||
            !(request.Id > 0))
        {
            return BadRequest(ApiResponse<ComponentResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, request.Id, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<ComponentResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var updatedComponent = _componentService.UpdateComponent(request.Id, request.Name, request.Color, request.Type, request.StationId, request.GpioPin);
        
        if (updatedComponent.IsFailure) 
        {
            response = ApiResponse<ComponentResponse>.Failure(updatedComponent.ErrorMessage);
            return BadRequest(response);
        }
        
        var formattedResponse = ComponentResponseConverter.ConvertComponentToComponentResponse(updatedComponent.Value);
        response = ApiResponse<ComponentResponse>.Success(formattedResponse);

        return Ok(response);
    }

    [HttpGet(nameof(GetFromStation))]
    public ActionResult<ApiResponse<ComponentListResponse>> GetFromStation(int stationId, bool includeComponents = false)
    {
        ApiResponse<ComponentListResponse> response;

        if (!(stationId > 0))
        {
            return BadRequest(ApiResponse<ComponentListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Station>(this, stationId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<ComponentListResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var componentList = _componentService.GetFromStation(stationId, includeComponents);

        if (componentList.IsFailure)
        {
            response = ApiResponse<ComponentListResponse>.Failure(componentList.ErrorMessage);
            return BadRequest(response);
        }

        ComponentListResponse formattedResponse = ComponentListResponseConverter.ConvertComponentListToComponentListResponse(componentList.Value);
        response = ApiResponse<ComponentListResponse>.Success(formattedResponse);
            
        return Ok(response);
    }

    [HttpGet(nameof(GetById))]
    public ActionResult<ApiResponse<ComponentResponse>> GetById(int componentId)
    {
        ApiResponse<ComponentResponse> response;

        if (!(componentId > 0))
        {
            return BadRequest(ApiResponse<ComponentResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, componentId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<ComponentResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var component = _componentService.GetById(componentId);

        if(component.IsFailure)
        {
            response = ApiResponse<ComponentResponse>.Failure(component.ErrorMessage);
            return BadRequest(response);
        }

        var formattedResponse = ComponentResponseConverter.ConvertComponentToComponentResponse(component.Value);
        response = ApiResponse<ComponentResponse>.Success(formattedResponse);
       
        return Ok(response);
    }

    [HttpPut(nameof(ActuatorCommand))]
    public async Task<ActionResult<ApiResponse<EmptyResponse>>> ActuatorCommand(ActuatorCommandRequest request)
    {
        ApiResponse<EmptyResponse> response;

        if (!(request.ComponentGpioPin > 0) ||
            !(request.StationId > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Station>(this, request.StationId))
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        try
        {
            bool isActionExecuted = await _mqttService.SendActuatorCommand(request.StationId,
                                                                     request.ComponentGpioPin,
                                                                     request.ComponentValueUpdate);
            if (isActionExecuted)
            {
                response = ApiResponse<EmptyResponse>.Success(null);
            }
            else
            {
                response = ApiResponse<EmptyResponse>.Failure(BaseResponses.INTERNAL_ERROR);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            response = ApiResponse<EmptyResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }
    }
}
