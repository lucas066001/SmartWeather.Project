namespace SmartWeather.Api.Controllers.Station.Dtos;

public class StationMeasurePointsResponse
{
    public int StationId { get; set; }
    public List<int> MeasurePointsIds { get; set; } = null!;
}
