using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.Station;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.ComponentDatas;

namespace SmartWeather.Repositories.ComponentDatas;

public class ComponentDataRepository(SmartWeatherContext context) : IComponentDataRepository
{
    public IEnumerable<ComponentData> GetFromComponent(int idComponent, DateTime? startPeriod = null, DateTime? endPeriod = null)
    {
        IEnumerable<ComponentData> componentDatasRetreived = null!;
        try
        {
            if (startPeriod != null && endPeriod != null)
            {
                componentDatasRetreived = context.ComponentDatas.Where(cd => cd.ComponentId == idComponent && (cd.DateTime >= startPeriod && cd.DateTime <= endPeriod)).ToList();
            }
            else
            {
                componentDatasRetreived = context.ComponentDatas.Where(cd => cd.ComponentId == idComponent).ToList();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive components from station in database : " + ex.Message);
        }

        return componentDatasRetreived;
    }
}
