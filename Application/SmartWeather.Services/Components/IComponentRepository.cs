namespace SmartWeather.Services.Components;

using SmartWeather.Entities.Component;
using SmartWeather.Entities.Common.Exceptions;

public interface IComponentRepository
{
    /// <summary>
    /// Retreive Components from a given Station.
    /// </summary>
    /// <param name="stationId">Int representing Station unique Id.</param>
    /// <returns>List of Component.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Component> GetFromStation(int stationId);

    /// <summary>
    /// Retreive Component based on it's unique Id.
    /// Possibility to include corresponding Station.
    /// </summary>
    /// <param name="componentId">Int representing Component unique Id.</param>
    /// <param name="includeStation">Bool indicating to include corresponding Station.</param>
    /// <returns>Component.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public Component GetById(int componentId, bool includeStation);
}
