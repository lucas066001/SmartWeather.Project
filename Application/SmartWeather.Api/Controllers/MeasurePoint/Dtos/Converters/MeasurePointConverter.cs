namespace SmartWeather.Api.Controllers.MeasurePoint.Dtos.Converters;

using SmartWeather.Api.Controllers.Component.Dtos;
using SmartWeather.Api.Controllers.Component.Dtos.Converters;
using SmartWeather.Entities.MeasurePoint;

public class MeasurePointConverter
{
    public static MeasurePointResponse ConvertMeasurePointToMeasurePointResponse(MeasurePoint measurePoint)
    {
        return new MeasurePointResponse()
        {
            Id = measurePoint.Id,
            Name = measurePoint.Name,
            Color = measurePoint.Color,
            Unit = (int)measurePoint.Unit,
            ComponentId = measurePoint.ComponentId,
        };
    }

    public static MeasurePointListResponse ConvertMeasurePointListToMeasurePointListResponse(IEnumerable<MeasurePoint> measurePoints)
    {
        MeasurePointListResponse result = new MeasurePointListResponse()
        {
            MeasurePointList = new List<MeasurePointResponse>()
        };

        foreach (var mp in measurePoints)
        {
            result.MeasurePointList.Add(ConvertMeasurePointToMeasurePointResponse(mp));
        }
        return result;
    }

    public static List<MeasurePointResponse> ConvertMeasurePointListToMeasurePointList(IEnumerable<MeasurePoint> measurePoints)
    {
        var result = new List<MeasurePointResponse>();

        foreach (var mp in measurePoints)
        {
            result.Add(ConvertMeasurePointToMeasurePointResponse(mp));
        }
        return result;
    }
}
