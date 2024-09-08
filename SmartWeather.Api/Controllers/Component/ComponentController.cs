namespace SmartWeather.Api.Controllers.Component;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Components;
using SmartWeather.Api.Controllers.Component.Dtos;
using SmartWeather.Entities.Component;
using SmartWeather.Api.Controllers.Component.Dtos.Converters;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Api.Controllers.Station.Dtos;
using SmartWeather.Services.Stations;
using SmartWeather.Services.Mqtt;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ComponentController : ControllerBase
{
    private readonly ComponentService _componentService;
    private readonly MqttService _mqttService;

    public ComponentController(ComponentService componentService,
                                MqttService mqttService)
    {
        _mqttService = mqttService;
        _componentService = componentService;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<ComponentResponse>> Create(ComponentCreateRequest request)
    {
        ApiResponse<ComponentResponse> response;
        ComponentResponse formattedResponse;

        if (String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.Color) ||
            !(request.GpioPin >= 0) ||
            !(request.StationId > 0))
        {
            return BadRequest(ApiResponse<ComponentResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            Component createdComponent = _componentService.AddNewComponent(request.Name, request.Color, request.Unit, request.Type, request.StationId, request.GpioPin);
            formattedResponse = ComponentResponseConverter.ConvertComponentToComponentResponse(createdComponent);
            response = ApiResponse<ComponentResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idComponent)
    {
        // Later will need to restrict this to admin or current token user privilege
        ApiResponse<EmptyResponse> response;

        if (!(idComponent > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            bool isUserDeleted = _componentService.DeleteComponent(idComponent);
            if (isUserDeleted)
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

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<ComponentResponse>> Update(ComponentUpdateRequest request)
    {
        ApiResponse<ComponentResponse> response;
        ComponentResponse formattedResponse;

        if (String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.Color) ||
            !(request.GpioPin >= 0) ||
            !(request.StationId > 0))
        {
            return BadRequest(ApiResponse<ComponentResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            Component updatedComponent = _componentService.UpdateComponent(request.Id, request.Name, request.Color, request.Unit, request.Type, request.StationId, request.GpioPin);
            formattedResponse = ComponentResponseConverter.ConvertComponentToComponentResponse(updatedComponent);
            response = ApiResponse<ComponentResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(nameof(GetFromStation))]
    public ActionResult<ApiResponse<ComponentListResponse>> GetFromStation(int stationId)
    {
        ApiResponse<ComponentListResponse> response;

        if (!(stationId > 0))
        {
            return BadRequest(ApiResponse<ComponentListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            IEnumerable<Component> componentList = _componentService.GetFromStation(stationId);
            if (componentList.Any())
            {
                ComponentListResponse formattedResponse = ComponentListResponseConverter.ConvertComponentListToComponentListResponse(componentList);
                response = ApiResponse<ComponentListResponse>.Success(formattedResponse);
            }
            else
            {
                response = ApiResponse<ComponentListResponse>.Success(null);
            }

        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

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

        try
        {
            Component component = _componentService.GetById(componentId);
            ComponentResponse formattedResponse = ComponentResponseConverter.ConvertComponentToComponentResponse(component);
            response = ApiResponse<ComponentResponse>.Success(formattedResponse);

        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut(nameof(ActuatorCommand))]
    public async Task<ActionResult<ApiResponse<EmptyResponse>>> ActuatorCommand(ActuatorCommandRequest request)
    {
        ApiResponse<EmptyResponse> response;

        if (!(request.ComponentId > 0) ||
            !(request.StationId > 0)
            )
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            bool isActionExecuted = await _mqttService.SendActuatorCommand(request.StationId,
                                                                     request.ComponentId,
                                                                     request.ComponentValueUpdate);
            if (isActionExecuted)
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
            response = ApiResponse<EmptyResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }
}
