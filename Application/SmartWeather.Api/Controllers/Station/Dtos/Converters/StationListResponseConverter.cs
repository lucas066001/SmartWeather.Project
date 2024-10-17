namespace SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Entities.Station;

public class StationListResponseConverter
{
    public static StationListResponse ConvertStationListToStationListResponse(IEnumerable<Station> stations)
    {
        StationListResponse result = new StationListResponse() { 
            StationList = new List<StationResponse>()
        };

        foreach (var station in stations)
        {
            result.StationList.Add(StationResponseConverter.ConvertStationToStationResponse(station));
        }
        return result;
    }
}
