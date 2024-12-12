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
    IEnumerable<MeasurePoint> GetFromComponent(int idComponent);

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
    bool IsOwnerOfMeasurePoint(int userId, int measurePointId);
}
