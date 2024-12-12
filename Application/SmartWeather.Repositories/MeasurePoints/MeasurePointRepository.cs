namespace SmartWeather.Repositories.MeasurePoints;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.MeasurePoints;


public class MeasurePointRepository(SmartWeatherReadOnlyContext readOnlyContext) : IMeasurePointRepository
{
    /// <summary>
    /// Retreive all MeasurePoint from Component.
    /// </summary>
    /// <param name="idComponent">Int representing Component unique Id that contains desired MeasurePoint.</param>
    /// <returns>List of MeasurePoint.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        var measurePointsRetreived = readOnlyContext.MeasurePoints.Where(cd => cd.ComponentId == idComponent).AsEnumerable();
        return measurePointsRetreived ?? throw new EntityFetchingException();
    }

    /// <summary>
    /// Check wether or not a User owns a MeasurePoint.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="measurePointId">Int representing MeasurePoint unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the MeasurePoint.
    /// (True : Owner | False : Not owner)
    /// </returns>
    /// <exception cref="EntityFetchingException">Thrown if no MeasurePoint can be found using given Id.</exception>
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
