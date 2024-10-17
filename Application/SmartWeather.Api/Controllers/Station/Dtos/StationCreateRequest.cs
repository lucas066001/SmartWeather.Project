namespace SmartWeather.Api.Controllers.Station.Dtos;

public class StationCreateRequest
{
    public required string Name { get; set; }
    public required string MacAddress { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public int UserId { get; set; }
}
