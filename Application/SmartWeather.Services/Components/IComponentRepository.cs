namespace SmartWeather.Services.Components;
using SmartWeather.Entities.Component;

public interface IComponentRepository
{
    public IEnumerable<Component> GetFromStation(int stationId);
}
