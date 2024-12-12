namespace SmartWeather.Repositories.MeasurePoints;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.MeasurePoints;

public class MeasurePointRepository(SmartWeatherReadOnlyContext readOnlyContext) : IMeasurePointRepository
{
    public MeasurePoint GetById(int measurePointId, bool includeComponent, bool includeStation)
    {
        if (includeStation)
        {
            var measurePointRetreived = readOnlyContext.MeasurePoints
                                                        .Include(mp => mp.Component)
                                                        .ThenInclude(c => c.Station)
                                                        .Where(mp => mp.Id == measurePointId)
                                                        .FirstOrDefault();
            return measurePointRetreived ?? throw new EntityFetchingException();
        }
        else if (includeComponent)
        {
            var measurePointRetreived = readOnlyContext.MeasurePoints
                                                       .Include(mp => mp.Component)
                                                       .Where(mp => mp.Id == measurePointId)
                                                       .FirstOrDefault();
            return measurePointRetreived ?? throw new EntityFetchingException();
        }
        else
        {
            var measurePointRetreived = readOnlyContext.MeasurePoints
                                                       .Where(mp => mp.Id == measurePointId)
                                                       .FirstOrDefault();
            return measurePointRetreived ?? throw new EntityFetchingException();
        }
    }

    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        var measurePointsRetreived = readOnlyContext.MeasurePoints.Where(cd => cd.ComponentId == idComponent).AsEnumerable();
        return measurePointsRetreived ?? throw new EntityFetchingException();
    }

    public bool IsOwnerOfMeasurePoint(int userId, int measurePointId)
    {
        var measurePointRetreived = readOnlyContext.MeasurePoints
                                        .Where(mp => mp.Id == measurePointId)
                                        .Include(mp => mp.Component)
                                        .ThenInclude(cp => cp.Station)
                                        .FirstOrDefault();
        
        if(measurePointRetreived == null) throw new EntityFetchingException();

        return measurePointRetreived.Component.Station.UserId == userId;
    }
}
