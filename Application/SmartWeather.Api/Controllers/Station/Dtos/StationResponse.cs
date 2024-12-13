using SmartWeather.Api.Controllers.Component.Dtos;
using SmartWeather.Entities.Station;

namespace SmartWeather.Api.Controllers.Station.Dtos;

public class StationResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public StationType Type { get; set; }
    public int? UserId { get; set; }
    public List<ComponentResponse>? Components { get; set; }
}
