namespace SmartWeather.Services.ComponentDatas;

using SmartWeather.Entities.ComponentData;
using SmartWeather.Services.Repositories;

public class ComponentDataService (IRepository<ComponentData> componentDataBaseRepository, IComponentDataRepository componentDataRepository)
{
    public ComponentData AddNewComponentData(int componentId, int value, DateTime dateTime)
    {
        ComponentData ComponentDataToCreate = new(componentId, value, dateTime);
        return componentDataBaseRepository.Create(ComponentDataToCreate);
    }

    public bool DeleteComponentData(int idComponentData)
    {
        return componentDataBaseRepository.Delete(idComponentData) != null;
    }

    public ComponentData UpdateComponentData(int id, int componentId, int value, DateTime dateTime)
    {
        ComponentData ComponentDataToUpdate = new(componentId, value, dateTime)
        {
            Id = id
        };
        return componentDataBaseRepository.Update(ComponentDataToUpdate);
    }

    public IEnumerable<ComponentData> GetFromComponent(int idComponent, DateTime? startPeriod, DateTime? endPeriod)
    {
        return componentDataRepository.GetFromComponent(idComponent, startPeriod, endPeriod);
    }
}
