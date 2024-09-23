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
public class MeasureDataController : ControllerBase
{
    private readonly MeasureDataService _componentDataService;

    public MeasureDataController(MeasureDataService componentDataService)
    {
        _componentDataService = componentDataService;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<MeasureDataResponse>> Create(MeasureDataCreateRequest request)
    {
        ApiResponse<MeasureDataResponse> response;
        MeasureDataResponse formattedResponse;

        if (!(request.MeasurePointId > 0))
        {
            return BadRequest(ApiResponse<MeasureDataResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            MeasureData createdComponentData = _componentDataService.AddNewComponentData(request.MeasurePointId, request.Value, request.DateTime);
            formattedResponse = MeasureDataResponseConverter.ConvertComponentDataToComponentDataResponse(createdComponentData);
            response = ApiResponse<MeasureDataResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasureDataResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
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
    public ActionResult<ApiResponse<MeasureDataResponse>> Update(MeasureDataUpdateRequest request)
    {
        ApiResponse<MeasureDataResponse> response;
        MeasureDataResponse formattedResponse;

        if (!(request.Id > 0) ||
            !(request.MeasurePointId > 0))
        {
            return BadRequest(ApiResponse<MeasureDataResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            MeasureData updatedComponentData = _componentDataService.UpdateComponentData(request.Id, request.MeasurePointId, request.Value, request.DateTime);
            formattedResponse = MeasureDataResponseConverter.ConvertComponentDataToComponentDataResponse(updatedComponentData);
            response = ApiResponse<MeasureDataResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasureDataResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(nameof(GetFromMeasurePoint))]
    public ActionResult<ApiResponse<MeasureDataListResponse>> GetFromMeasurePoint(int measurePointId, DateTime? startPeriod = null, DateTime? endPeriod = null)
    {
        ApiResponse<MeasureDataListResponse> response;

        if (!(measurePointId > 0))
        {
            return BadRequest(ApiResponse<MeasureDataListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            IEnumerable<MeasureData> componentDataList = _componentDataService.GetFromMeasurePoint(measurePointId, startPeriod, endPeriod);
            if (componentDataList.Any())
            {
                MeasureDataListResponse formattedResponse = MeasureDataListResponseConverter.ConvertComponentDataListToComponentDataListResponse(componentDataList);
                response = ApiResponse<MeasureDataListResponse>.Success(formattedResponse);
            }
            else
            {
                response = ApiResponse<MeasureDataListResponse>.Success(null);
            }

        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasureDataListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

}
