namespace SmartWeather.Services.Components;

using SmartWeather.Entities.Common;
using SmartWeather.Entities.Component;

public interface IComponentRepository
{
    /// <summary>
    /// Retreive Components from a given Station.
    /// </summary>
    /// <param name="stationId">Int representing Station unique Id.</param>
    /// <returns>Result containing list of Component.</returns>
    public Result<IEnumerable<Component>> GetFromStation(int stationId);

    /// <summary>
    /// Retreive Component based on it's unique Id.
    /// Possibility to include corresponding Station.
    /// </summary>
    /// <param name="componentId">Int representing Component unique Id.</param>
    /// <param name="includeStation">Bool indicating to include corresponding Station.</param>
    /// <returns>Result containing Component.</returns>
    public Result<Component> GetById(int componentId, bool includeStation);
}
