namespace SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Entities.Station;

public class StationMeasurePointsResponseConverter
{
    public static StationMeasurePointsResponse ConvertStationResponseToStationMeasurePointResponse(Station station)
    {
        try
        {
            return new()
            {
                StationId = station.Id,
                MeasurePointsIds = station.Components.SelectMany(c => c.MeasurePoints.Select(mp => mp.Id)).ToList()
            };
        }
        catch
        {
            return new();
        }

    }
}
