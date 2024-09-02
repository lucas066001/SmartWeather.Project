using SmartWeather.Entities.ComponentData;

namespace SmartWeather.Services.ComponentDatas;

public interface IComponentDataRepository
{
    IEnumerable<ComponentData> GetFromComponent(int idComponent, DateTime? startPeriod = null, DateTime? endPeriod = null);
}
