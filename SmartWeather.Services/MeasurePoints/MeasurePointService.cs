namespace SmartWeather.Services.MeasurePoints;

using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Services.Repositories;

public class MeasurePointService (IRepository<MeasurePoint> measurePointBaseRepository, IMeasurePointRepository measurePointRepository)
{
    public MeasurePoint AddNewMeasurePoint(string name, string color, int unit, int componentId)
    {
        MeasurePoint MeasurePointToCreate = new(name, color, unit, componentId);
        return measurePointBaseRepository.Create(MeasurePointToCreate);
    }

    public bool DeleteMeasurePoint(int idMeasurePoint)
    {
        return measurePointBaseRepository.Delete(idMeasurePoint) != null;
    }

    public MeasurePoint UpdateMeasurePoint(int id, string name, string color, int unit, int componentId)
    {
        MeasurePoint MeasurePointToUpdate = new(name, color, unit, componentId)
        {
            Id = id
        };
        return measurePointBaseRepository.Update(MeasurePointToUpdate);
    }

    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        return measurePointRepository.GetFromComponent(idComponent);
    }
}
