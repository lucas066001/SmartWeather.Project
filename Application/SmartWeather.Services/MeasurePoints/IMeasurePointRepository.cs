namespace SmartWeather.Services.MeasurePoints;

using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Common.Exceptions;

public interface IMeasurePointRepository
{
    /// <summary>
    /// Retreive all MeasurePoint from Component.
    /// </summary>
    /// <param name="idComponent">Int representing Component unique Id that contains desired MeasurePoint.</param>
    /// <returns>List of MeasurePoint.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent);

    /// <summary>
    /// Retreive MeasurePoint based on given Id.
    /// Possibility to include corresponding Component and Station.
    /// </summary>
    /// <param name="measurePointId"></param>
    /// <param name="includeComponent">Boll indicating to include corresponding Component.</param>
    /// <param name="includeStation">Boll indicating to include corresponding Station.</param>
    /// <returns>MeasurePoint.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public MeasurePoint GetById(int measurePointId, bool includeComponent, bool includeStation);
}
