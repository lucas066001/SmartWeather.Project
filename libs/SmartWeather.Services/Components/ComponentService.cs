namespace SmartWeather.Services.Components;

using SmartWeather.Entities.Common;
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
    /// <returns>Result containing newly created entity.</returns>
    public Result<Component> AddNewComponent (string name,
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
            return Result<Component>.Failure(string.Format(
                                                    ExceptionsBaseMessages.ENTITY_FORMAT,
                                                    nameof(Component),
                                                    ex.Message));
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
        return componentBaseRepository.Delete(idComponent).IsSuccess;
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
    /// <returns>Result containing modified component in database.</returns>
    public Result<Component> UpdateComponent(int id, string name, string color, int type, int stationId, int gpioPin)
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
            return Result<Component>.Failure(string.Format(
                                        ExceptionsBaseMessages.ENTITY_FORMAT,
                                        nameof(Component),
                                        ex.Message));
        }
    }

    /// <summary>
    /// Retreive Components from a given Station.
    /// </summary>
    /// <param name="idStation">Int representing Station unique Id.</param>
    /// <returns>Result containing list of Component.</returns>
    public Result<IEnumerable<Component>> GetFromStation(int idStation, bool includeComponents = false)
    {
        return componentRepository.GetFromStation(idStation, includeComponents);
    }

    /// <summary>
    /// Retreive Component based on it's Id.
    /// </summary>
    /// <param name="componentId">Int corresponding to Component unique Id.</param>
    /// <returns>Result containing desired Component.</returns>
    public Result<Component> GetById(int componentId)
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
        var component = componentRepository.GetById(idComponent, true);
        return component.IsSuccess && component.Value.Station.UserId == userId;
    }
}
