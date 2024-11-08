namespace SmartWeather.Services.MeasurePoints;

using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Services.Repositories;

public class MeasurePointService (IRepository<MeasurePoint> measurePointBaseRepository, IMeasurePointRepository measurePointRepository)
{
    public MeasurePoint AddNewMeasurePoint(int localId, string name, string color, int unit, int componentId)
    {
        MeasurePoint MeasurePointToCreate = new(localId, name, color, unit, componentId);
        return measurePointBaseRepository.Create(MeasurePointToCreate);
    }

    public bool DeleteMeasurePoint(int idMeasurePoint)
    {
        return measurePointBaseRepository.Delete(idMeasurePoint) != null;
    }

    public MeasurePoint UpdateMeasurePoint(int id, int localId, string name, string color, int unit, int componentId)
    {
        MeasurePoint MeasurePointToUpdate = new(localId, name, color, unit, componentId)
        {
            Id = id
        };
        return measurePointBaseRepository.Update(MeasurePointToUpdate);
    }

    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        return measurePointRepository.GetFromComponent(idComponent);
    }

    public bool IsOwnerOfMeasurePoint(int userId, int idMeasurePoint)
    {
        return measurePointRepository.IsOwnerOfMeasurePoint(userId, idMeasurePoint);
    }
}
