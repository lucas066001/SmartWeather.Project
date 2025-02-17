namespace SmartWeather.Api.Controllers.Station.Dtos;

public class StationUpdateRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public int UserId { get; set; }
}
