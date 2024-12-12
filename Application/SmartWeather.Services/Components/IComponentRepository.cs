namespace SmartWeather.Services.Components;
using SmartWeather.Entities.Component;

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
    /// Check wether or not a User owns a Component.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="idComponent">Int representing Component unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the Component.
    /// (True : Owner | False : Not owner)
    /// </returns>
    /// <exception cref="Exception"></exception>
    public bool IsOwnerOfComponent(int userId, int idComponent);
}
