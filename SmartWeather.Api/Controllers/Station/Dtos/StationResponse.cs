namespace SmartWeather.Api.Controllers.Station.Dtos;

public class StationResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public required string TopicLocation { get; set; }
    public required Entities.Station.StationType Type { get; set; }
    public int? UserId { get; set; }
}
