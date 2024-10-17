using SmartWeather.Api.Controllers.Station.Dtos;

namespace SmartWeather.Api.Controllers.Component.Dtos;

public class ComponentListResponse
{
    public List<ComponentResponse> ComponentList { get; set; } = null!;
}
