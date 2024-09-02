namespace SmartWeather.Api.Controllers.ComponentData;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Api.Controllers.ComponentData.Dtos;
using SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ComponentDataController : ControllerBase
{
    private readonly ComponentDataService _componentDataService;

    public ComponentDataController(ComponentDataService componentDataService)
    {
        _componentDataService = componentDataService;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<ComponentDataResponse>> Create(ComponentDataCreateRequest request)
    {
        ApiResponse<ComponentDataResponse> response;
        ComponentDataResponse formattedResponse;

        if (!(request.ComponentId > 0))
        {
            return BadRequest(ApiResponse<ComponentDataResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            ComponentData createdComponentData = _componentDataService.AddNewComponentData(request.ComponentId, request.Value, request.DateTime);
            formattedResponse = ComponentDataResponseConverter.ConvertComponentDataToComponentDataResponse(createdComponentData);
            response = ApiResponse<ComponentDataResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentDataResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idComponentData)
    {
        // Later will need to restrict this to admin or current token user privilege
        ApiResponse<EmptyResponse> response;

        if (!(idComponentData > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            bool isUserDeleted = _componentDataService.DeleteComponentData(idComponentData);
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
    public ActionResult<ApiResponse<ComponentDataResponse>> Update(ComponentDataUpdateRequest request)
    {
        ApiResponse<ComponentDataResponse> response;
        ComponentDataResponse formattedResponse;

        if (!(request.Id > 0) ||
            !(request.ComponentId > 0))
        {
            return BadRequest(ApiResponse<ComponentDataResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            ComponentData updatedComponentData = _componentDataService.UpdateComponentData(request.Id, request.ComponentId, request.Value, request.DateTime);
            formattedResponse = ComponentDataResponseConverter.ConvertComponentDataToComponentDataResponse(updatedComponentData);
            response = ApiResponse<ComponentDataResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentDataResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(nameof(GetFromComponent))]
    public ActionResult<ApiResponse<ComponentDataListResponse>> GetFromComponent(int componentId, DateTime? startPeriod = null, DateTime? endPeriod = null)
    {
        ApiResponse<ComponentDataListResponse> response;

        if (!(componentId > 0))
        {
            return BadRequest(ApiResponse<ComponentDataListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            IEnumerable<ComponentData> componentDataList = _componentDataService.GetFromComponent(componentId, startPeriod, endPeriod);
            if (componentDataList.Any())
            {
                ComponentDataListResponse formattedResponse = ComponentDataListResponseConverter.ConvertComponentDataListToComponentDataListResponse(componentDataList);
                response = ApiResponse<ComponentDataListResponse>.Success(formattedResponse);
            }
            else
            {
                response = ApiResponse<ComponentDataListResponse>.Success(null);
            }

        }
        catch (Exception ex)
        {
            response = ApiResponse<ComponentDataListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

}
