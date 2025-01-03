namespace SmartWeather.Services.MeasurePoints;

using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Common;

public interface IMeasurePointRepository
{
    /// <summary>
    /// Retreive all MeasurePoint from Component.
    /// </summary>
    /// <param name="idComponent">Int representing Component unique Id that contains desired MeasurePoint.</param>
    /// <returns>Result containing list of MeasurePoint.</returns>
    public Result<IEnumerable<MeasurePoint>> GetFromComponent(int idComponent);

    /// <summary>
    /// Retreive MeasurePoint based on given Id.
    /// Possibility to include corresponding Component and Station.
    /// </summary>
    /// <param name="measurePointId"></param>
    /// <param name="includeComponent">Bool indicating to include corresponding Component.</param>
    /// <param name="includeStation">Bool indicating to include corresponding Station.</param>
    /// <returns>Result containing measurePoint.</returns>
    public Result<MeasurePoint> GetById(int measurePointId, bool includeComponent, bool includeStation);
}
