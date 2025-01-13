namespace SmartWeather.Api.Controllers.ActivationPlan;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Api.Controllers.Component.Dtos.Converters;
using SmartWeather.Api.Controllers.Component.Dtos;
using SmartWeather.Api.Helpers;
using SmartWeather.Entities.User;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.ActivationPlan;
using SmartWeather.Services.Components;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.ActivationPlan;
using SmartWeather.Api.Controllers.ActivationPlan.Dtos;
using SmartWeather.Api.Controllers.ActivationPlan.Dtos.Converters;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ActivationPlanController : Controller
{
    private readonly ActivationPlanService _activationPlanService;
    private readonly AccessManagerHelper _accessManagerHelper;

    public ActivationPlanController(ActivationPlanService activationPlanService,
                                AccessManagerHelper accessManagerHelper)
    {
        _activationPlanService = activationPlanService;
        _accessManagerHelper = accessManagerHelper;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<ActivationPlanResponse>> Create(ActivationPlanCreateRequest request)
    {
        ApiResponse<ActivationPlanResponse> response;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            !(request.StartingDay >= 0 && request.StartingDay <= 6) ||
            !(request.Period.Ticks >= 1_000_000) ||
            !(request.Duration.Ticks >= 1_000_000) ||
            !(request.ComponentId > 0))
        {
            return BadRequest(ApiResponse<ActivationPlanResponse>.Failure(BaseResponses.ARGUMENT_ERROR,
                                                                     Status.VALIDATION_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, request.ComponentId, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<ActivationPlanResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var createdActivationPlan = _activationPlanService.AddNewActivationPlan(request.Name, request.StartingDay, request.Period, request.Duration, request.ComponentId);

        if (createdActivationPlan.IsFailure)
        {
            response = ApiResponse<ActivationPlanResponse>.Failure(createdActivationPlan.ErrorMessage);
            return BadRequest(response);
        }

        var formattedResponse = ActivationPlanResponseConverter.ConvertActivationPlanToActivationPlanResponse(createdActivationPlan.Value);
        response = ApiResponse<ActivationPlanResponse>.Success(formattedResponse);

        return Ok(response);

    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idActivationPlan)
    {
        ApiResponse<EmptyResponse> response;

        if (idActivationPlan <= 0)
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR,
                                                                 Status.VALIDATION_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<ActivationPlan>(this, idActivationPlan, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        bool isactivationPlanDeleted = _activationPlanService.DeleteActivationPlan(idActivationPlan);
        if (isactivationPlanDeleted)
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
    public ActionResult<ApiResponse<ActivationPlanResponse>> Update(ActivationPlanUpdateRequest request)
    {
        ApiResponse<ActivationPlanResponse> response;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            !(request.StartingDay >= 0 && request.StartingDay <= 6) ||
            !(request.Period.Ticks >= 1_000_000) ||
            !(request.Duration.Ticks >= 1_000_000) ||
            !(request.ComponentId > 0) ||
            !(request.Id > 0))
        {
            return BadRequest(ApiResponse<ActivationPlanResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<ActivationPlan>(this, request.Id, RoleAccess.ADMINISTRATORS) ||
            !_accessManagerHelper.ValidateUserAccess<Component>(this, request.ComponentId, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<ActivationPlanResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var updatedActivationPlan = _activationPlanService.UpdateComponent(request.Id, request.Name, request.StartingDay, request.Period, request.Duration, request.ComponentId);

        if (updatedActivationPlan.IsFailure)
        {
            response = ApiResponse<ActivationPlanResponse>.Failure(updatedActivationPlan.ErrorMessage);
            return BadRequest(response);
        }

        var formattedResponse = ActivationPlanResponseConverter.ConvertActivationPlanToActivationPlanResponse(updatedActivationPlan.Value);
        response = ApiResponse<ActivationPlanResponse>.Success(formattedResponse);

        return Ok(response);
    }

    [HttpGet(nameof(GetFromComponent))]
    public ActionResult<ApiResponse<ActivationPlanListResponse>> GetFromComponent(int componentId)
    {
        ApiResponse<ActivationPlanListResponse> response;

        if (!(componentId > 0))
        {
            return BadRequest(ApiResponse<ComponentListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<Component>(this, componentId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<ActivationPlanListResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var activationPlanList = _activationPlanService.GetFromComponent(componentId);

        if (activationPlanList.IsFailure)
        {
            response = ApiResponse<ActivationPlanListResponse>.Failure(activationPlanList.ErrorMessage);
            return BadRequest(response);
        }

        ActivationPlanListResponse formattedResponse = ActivationPlanListResponseConverter.ConvertActivationPlanListToActivationPlanResponseList(activationPlanList.Value);
        response = ApiResponse<ActivationPlanListResponse>.Success(formattedResponse);

        return Ok(response);
    }

    [HttpGet(nameof(GetById))]
    public ActionResult<ApiResponse<ActivationPlanResponse>> GetById(int activationPlanId)
    {
        ApiResponse<ActivationPlanResponse> response;

        if (!(activationPlanId > 0))
        {
            return BadRequest(ApiResponse<ActivationPlanResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<ActivationPlan>(this, activationPlanId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<ActivationPlanResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var activationPlan = _activationPlanService.GetById(activationPlanId);

        if (activationPlan.IsFailure)
        {
            response = ApiResponse<ActivationPlanResponse>.Failure(activationPlan.ErrorMessage);
            return BadRequest(response);
        }

        var formattedResponse = ActivationPlanResponseConverter.ConvertActivationPlanToActivationPlanResponse(activationPlan.Value);
        response = ApiResponse<ActivationPlanResponse>.Success(formattedResponse);

        return Ok(response);
    }
}
