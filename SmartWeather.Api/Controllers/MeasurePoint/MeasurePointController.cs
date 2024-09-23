namespace SmartWeather.Api.Controllers.MeasurePoint;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;
using SmartWeather.Api.Controllers.ComponentData.Dtos;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Api.Controllers.MeasurePoint.Dtos;
using SmartWeather.Api.Controllers.MeasurePoint.Dtos.Converters;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeasurePointController : Controller
{
    private readonly MeasurePointService _measurePointService;

    public MeasurePointController(MeasurePointService measurePointService)
    {
        _measurePointService = measurePointService;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<MeasurePointResponse>> Create(MeasurePointCreateRequest request)
    {
        ApiResponse<MeasurePointResponse> response;
        MeasurePointResponse formattedResponse;

        if (String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.Color) ||
            !(request.ComponentId > 0) ||
            !(request.LocalId > 0) ||
            !(request.Unit >= 0))
        {
            return BadRequest(ApiResponse<MeasurePointResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            MeasurePoint createdComponentData = _measurePointService.AddNewMeasurePoint(request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);
            formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(createdComponentData);
            response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

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

        try
        {
            bool isMeasurePointDeleted = _measurePointService.DeleteMeasurePoint(idMeasurePoint);
            if (isMeasurePointDeleted)
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
    public ActionResult<ApiResponse<MeasurePointResponse>> Update(MeasurePointUpdateRequest request)
    {
        ApiResponse<MeasurePointResponse> response;
        MeasurePointResponse formattedResponse;

        if (!(request.Id > 0) ||
            String.IsNullOrWhiteSpace(request.Name) ||
            String.IsNullOrWhiteSpace(request.Color) ||
            !(request.ComponentId > 0) ||
            !(request.LocalId > 0) || 
            !(request.Unit >= 0))
        {
            return BadRequest(ApiResponse<MeasurePointResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            MeasurePoint updatedMeasurePoint = _measurePointService.UpdateMeasurePoint(request.Id, request.LocalId, request.Name, request.Color, request.Unit, request.ComponentId);
            formattedResponse = MeasurePointConverter.ConvertMeasurePointToMeasurePointResponse(updatedMeasurePoint);
            response = ApiResponse<MeasurePointResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasurePointResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

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

        try
        {
            IEnumerable<MeasurePoint> measurePointList = _measurePointService.GetFromComponent(componentId);
            if (measurePointList.Any())
            {
                MeasurePointListResponse formattedResponse = MeasurePointConverter.ConvertMeasurePointListToMeasurePointListResponse(measurePointList);
                response = ApiResponse<MeasurePointListResponse>.Success(formattedResponse);
            }
            else
            {
                response = ApiResponse<MeasurePointListResponse>.Success(null);
            }

        }
        catch (Exception ex)
        {
            response = ApiResponse<MeasurePointListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

}
