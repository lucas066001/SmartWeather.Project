﻿namespace SmartWeather.Api.Controllers.MeasureData;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Api.Controllers.ComponentData.Dtos;
using SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;
using SmartWeather.Api.Helpers;
using SmartWeather.Entities.User;
using SmartWeather.Entities.MeasurePoint;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeasureDataController : ControllerBase
{
    private readonly MeasureDataService _componentDataService;
    private readonly AccessManagerHelper _accessManagerHelper;

    public MeasureDataController(MeasureDataService componentDataService, AccessManagerHelper accessManagerHelper)
    {
        _componentDataService = componentDataService;
        _accessManagerHelper = accessManagerHelper;
    }

    [HttpGet(nameof(GetFromMeasurePoint))]
    public ActionResult<ApiResponse<MeasureDataListResponse>> GetFromMeasurePoint(int measurePointId, DateTime startPeriod, DateTime endPeriod)
    {
        ApiResponse<MeasureDataListResponse> response;

        if (!(measurePointId > 0))
        {
            return BadRequest(ApiResponse<MeasureDataListResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<MeasurePoint>(this, measurePointId, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<MeasureDataListResponse>.Failure(BaseResponses.AUTHORIZATION_ERROR, Status.AUTHORIZATION_ERROR);
            return Unauthorized(response);
        }

        var componentDataList = _componentDataService.GetFromMeasurePoint(measurePointId, startPeriod, endPeriod);

        if (componentDataList.IsFailure)
        {
            response = ApiResponse<MeasureDataListResponse>.Failure(componentDataList.ErrorMessage);
            return BadRequest(response);
        }
        
        MeasureDataListResponse formattedResponse = MeasureDataListResponseConverter.ConvertComponentDataListToComponentDataListResponse(componentDataList.Value);
        response = ApiResponse<MeasureDataListResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

}
