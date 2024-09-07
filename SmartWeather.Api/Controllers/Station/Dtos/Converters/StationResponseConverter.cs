namespace SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Entities.Station;

public class StationResponseConverter
{
    public static StationResponse ConvertStationToStationResponse(Station station)
    {
        return new StationResponse()
        {
            Id = station.Id,
            Name = station.Name,
            Type = station.Type,
            UserId = station.UserId,
            Longitude = station.Longitude,
            Latitude = station.Latitude
        };
    }
}
