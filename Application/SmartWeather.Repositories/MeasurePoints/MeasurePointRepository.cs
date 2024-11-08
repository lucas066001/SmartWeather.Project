using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.MeasurePoints;

namespace SmartWeather.Repositories.MeasurePoints;

public class MeasurePointRepository(SmartWeatherReadOnlyContext readOnlyContext) : IMeasurePointRepository
{
    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        IEnumerable<MeasurePoint> measurePointsRetreived = null!;

        try
        {
            measurePointsRetreived = readOnlyContext.MeasurePoints.Where(cd => cd.ComponentId == idComponent).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive measurePoints from component in database : " + ex.Message);
        }


        return measurePointsRetreived;
    }

    public bool IsOwnerOfMeasurePoint(int userId, int measurePointId)
    {
        try
        {
            var measurePointRetreived = readOnlyContext.MeasurePoints
                                            .Where(mp => mp.Id == measurePointId)
                                            .Include(mp => mp.Component)
                                            .ThenInclude(cp => cp.Station)
                                            .FirstOrDefault();

            return measurePointRetreived != null && measurePointRetreived.Component.Station.UserId == userId;
        }
        catch 
        {
            throw new Exception("Unable to retreive measurePoints");
        }
    }
}
