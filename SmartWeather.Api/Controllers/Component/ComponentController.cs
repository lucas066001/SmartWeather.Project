namespace SmartWeather.Api.Controllers.Component;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Components;
using SmartWeather.Api.Controllers.Component.Dtos;
using SmartWeather.Entities.Component;
using SmartWeather.Api.Controllers.Component.Dtos.Converters;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ComponentController : ControllerBase
{
    private readonly ComponentService _componentService;

    public ComponentController(ComponentService componentService)
    {
        _componentService = componentService;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<ComponentResponse>> Create(ComponentCreateRequest request)
    {
        ApiResponse<ComponentResponse> response;
        ComponentResponse formattedResponse;

        if (String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.Color) ||
            !(request.StationId > 0))
        {
            return BadRequest(ApiResponse<ComponentResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            Component createdComponent = _componentService.AddNewComponent(request.Name, request.Color, request.Unit, request.Type, request.StationId);
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
}
