namespace SmartWeather.Services.ComponentDatas;

using SmartWeather.Entities.ComponentData;
using SmartWeather.Services.Repositories;

public class MeasureDataService (IMeasureDataRepository measureDataRepository)
{
    //public MeasureData AddNewMeasureData(int measurePointId, float value, DateTime dateTime)
    //{
    //    MeasureData ComponentDataToCreate = new(measurePointId, value, dateTime);
    //    return measureDataBaseRepository.Create(ComponentDataToCreate);
    //}

    public void InsertMeasureData(int measurePointId, float value, DateTime dateTime)
    {
        MeasureData ComponentDataToCreate = new(measurePointId, value, dateTime);
        measureDataRepository.Create(ComponentDataToCreate);
    }

    //public bool DeleteComponentData(int idComponentData)
    //{
    //    return measureDataBaseRepository.Delete(idComponentData) != null;
    //}

    //public MeasureData UpdateComponentData(int id, int componentId, int value, DateTime dateTime)
    //{
    //    MeasureData ComponentDataToUpdate = new(componentId, value, dateTime)
    //    {
    //        Id = id
    //    };
    //    return measureDataBaseRepository.Update(ComponentDataToUpdate);
    //}

    public IEnumerable<MeasureData> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod)
    {
        return measureDataRepository.GetFromMeasurePoint(idMeasurePoint, startPeriod, endPeriod).Result;
    }
}
