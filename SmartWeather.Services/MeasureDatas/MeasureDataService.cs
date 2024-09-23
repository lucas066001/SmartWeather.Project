namespace SmartWeather.Services.ComponentDatas;

using SmartWeather.Entities.ComponentData;
using SmartWeather.Services.Repositories;

public class MeasureDataService (IRepository<MeasureData> componentDataBaseRepository, IMeasureDataRepository componentDataRepository)
{
    public MeasureData AddNewComponentData(int componentId, float value, DateTime dateTime)
    {
        MeasureData ComponentDataToCreate = new(componentId, value, dateTime);
        return componentDataBaseRepository.Create(ComponentDataToCreate);
    }

    public bool DeleteComponentData(int idComponentData)
    {
        return componentDataBaseRepository.Delete(idComponentData) != null;
    }

    public MeasureData UpdateComponentData(int id, int componentId, int value, DateTime dateTime)
    {
        MeasureData ComponentDataToUpdate = new(componentId, value, dateTime)
        {
            Id = id
        };
        return componentDataBaseRepository.Update(ComponentDataToUpdate);
    }

    public IEnumerable<MeasureData> GetFromMeasurePoint(int idMeasurePoint, DateTime? startPeriod, DateTime? endPeriod)
    {
        return componentDataRepository.GetFromMeasurePoint(idMeasurePoint, startPeriod, endPeriod);
    }
}
