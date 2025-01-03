namespace SmartWeather.Repositories.MeasurePoints;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Entities.Common;

public class MeasurePointRepository(SmartWeatherReadOnlyContext readOnlyContext) : IMeasurePointRepository
{
    public Result<MeasurePoint> GetById(int measurePointId, bool includeComponent, bool includeStation)
    {
        MeasurePoint? measurePointRetreived = null;
        if (includeStation)
        {
            measurePointRetreived = readOnlyContext.MeasurePoints
                                                        .Include(mp => mp.Component)
                                                        .ThenInclude(c => c.Station)
                                                        .Where(mp => mp.Id == measurePointId)
                                                        .FirstOrDefault();
        }
        else if (includeComponent)
        {
            measurePointRetreived = readOnlyContext.MeasurePoints
                                                       .Include(mp => mp.Component)
                                                       .Where(mp => mp.Id == measurePointId)
                                                       .FirstOrDefault();
        }
        else
        {
            measurePointRetreived = readOnlyContext.MeasurePoints
                                                       .Where(mp => mp.Id == measurePointId)
                                                       .FirstOrDefault();
        }
        return measurePointRetreived != null ? 
                    Result<MeasurePoint>.Success(measurePointRetreived) : 
                    Result<MeasurePoint>.Failure(string.Format(
                                                            ExceptionsBaseMessages.ENTITY_FETCH,
                                                            nameof(MeasurePoint)));
    }

    public Result<IEnumerable<MeasurePoint>> GetFromComponent(int idComponent)
    {
        var measurePointsRetreived = readOnlyContext.MeasurePoints
                                                    .Where(cd => cd.ComponentId == idComponent)
                                                    .AsEnumerable();
        return measurePointsRetreived != null ?
            Result<IEnumerable<MeasurePoint>>.Success(measurePointsRetreived) :
            Result<IEnumerable<MeasurePoint>>.Failure(string.Format(
                                                        ExceptionsBaseMessages.ENTITY_FETCH,
                                                        nameof(MeasurePoint)));
    }
}
