namespace SmartWeather.Services.Components;

using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Component.Exceptions;
using SmartWeather.Services.Repositories;

public class ComponentService (IRepository<Component> componentBaseRepository, IComponentRepository componentRepository)
{
    /// <summary>
    /// Create new Componet in database based on given infos.
    /// </summary>
    /// <param name="name">String representing component name.</param>
    /// <param name="color">String representing component color.</param>
    /// <param name="type">Int representing component type.</param>
    /// <param name="stationId">Int representing station id that hold component.</param>
    /// <param name="gpioPin">Int representing embded component pin.</param>
    /// <returns></returns>
    /// <exception cref="EntityCreationException">Thrown if Component informations doesn't meet requirements. (eg:color format)</exception>
    /// <exception cref="EntitySavingException">Thrown if creation doesn't execute properly.</exception>
    public Component AddNewComponent (string name,
                                      string color, 
                                      int type, 
                                      int stationId, 
                                      int gpioPin)
    {
        try
        {
            Component componentToCreate = new(name, color, type, stationId, gpioPin);
            return componentBaseRepository.Create(componentToCreate);
        }
        catch (Exception ex) when (ex is InvalidComponentNameException ||
                                   ex is InvalidComponentColorException ||
                                   ex is InvalidComponentTypeException ||
                                   ex is InvalidComponentStationIdException ||
                                   ex is InvalidComponentGpioPinException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Delete Component based on given id.
    /// </summary>
    /// <param name="idComponent">Int that correspond to Component unique Id.</param>
    /// <returns>
    /// Bool indicating deletion Component success.
    /// (True : Success | False : Failure)
    /// </returns>
    public bool DeleteComponent(int idComponent)
    {
        try
        {
            return componentBaseRepository.Delete(idComponent) != null;
        }
        catch (Exception ex) when (ex is EntityFetchingException ||
                                   ex is EntitySavingException)
        {
            return false;
        }
    }

    /// <summary>
    /// Modify Component in database.
    /// </summary>
    /// <param name="id">Int representing Component unique Id.</param>
    /// <param name="name">String representing component name.</param>
    /// <param name="color">String representing component color.</param>
    /// <param name="type">Int representing component type.</param>
    /// <param name="stationId">Int representing station id that hold component.</param>
    /// <param name="gpioPin">Int representing embded component pin.</param>
    /// <returns></returns>
    /// <exception cref="EntitySavingException">Thrown if unable to create Component. Mainly due to format error (e.g: color format not hex).</exception>
    /// <exception cref="EntityCreationException">Thrown if error occurs during database update.</exception>
    public Component UpdateComponent(int id, string name, string color, int type, int stationId, int gpioPin)
    {
        try
        {
            Component componentToUpdate = new(name, color, type, stationId, gpioPin)
            {
                Id = id
            };
            return componentBaseRepository.Update(componentToUpdate);
        }
        catch(Exception ex) when (ex is InvalidComponentGpioPinException ||
                                  ex is InvalidComponentNameException ||
                                  ex is InvalidComponentColorException ||
                                  ex is InvalidComponentTypeException ||
                                  ex is InvalidComponentStationIdException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Retreive Components from a given Station.
    /// </summary>
    /// <param name="idStation">Int representing Station unique Id.</param>
    /// <returns>List of Component.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Component> GetFromStation(int idStation)
    {
        return componentRepository.GetFromStation(idStation);
    }

    /// <summary>
    /// Retreive Component based on it's Id.
    /// </summary>
    /// <param name="componentId">Int corresponding to Component unique Id.</param>
    /// <returns>A Component object.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public Component GetById(int componentId)
    {
        return componentBaseRepository.GetById(componentId);
    }

    /// <summary>
    /// Check wether or not a User owns a Component.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="idComponent">Int representing Component unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the Component.
    /// (True : Owner | False : Not owner)
    /// </returns>
    public bool IsOwnerOfComponent(int userId, int idComponent)
    {
        try
        {
            var component = componentRepository.GetById(idComponent, true);
            return component.Station.UserId == userId;
        }
        catch (EntityFetchingException)
        {
            return false;
        }
    }
}
