namespace SmartWeather.Services.MeasurePoints;

using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.MeasurePoint.Exceptions;
using SmartWeather.Services.Repositories;

public class MeasurePointService (IRepository<MeasurePoint> measurePointBaseRepository, IMeasurePointRepository measurePointRepository)
{
    /// <summary>
    /// Create new MeasurePoint in database.
    /// </summary>
    /// <param name="localId">Int representing measure point embded id.</param>
    /// <param name="name">String representing measure point name.</param>
    /// <param name="color">String representing measure point color.</param>
    /// <param name="unit">Int representing measure point unit.</param>
    /// <param name="componentId">Int representing measure point component holder id.</param>
    /// <returns>Created MeasurePoint from database.</returns>
    /// <exception cref="EntityCreationException">Thrown if MeasurePoint informations doesn't meet requirements. (eg:color format)</exception>
    /// <exception cref="EntitySavingException">Thrown if creation doesn't execute properly.</exception>
    public MeasurePoint AddNewMeasurePoint (int localId,
                                            string name, 
                                            string color, 
                                            int unit, 
                                            int componentId)
    {
        try
        {
            MeasurePoint MeasurePointToCreate = new(localId, name, color, unit, componentId);
            return measurePointBaseRepository.Create(MeasurePointToCreate);
        }
        catch (Exception ex) when (ex is InvalidMeasurePointLocalIdException ||
                           ex is InvalidMeasurePointNameException ||
                           ex is InvalidMeasurePointColorException ||
                           ex is InvalidMeasurePointUnitException ||
                           ex is InvalidMeasurePointComponentIdException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Delete MeasurePoint based on given id.
    /// </summary>
    /// <param name="idMeasurePoint">Int that correspond to MeasurePoint unique Id.</param>
    /// <returns>
    /// Bool indicating deletion MeasurePoint success.
    /// (True : Success | False : Failure)
    /// </returns>
    public bool DeleteMeasurePoint(int idMeasurePoint)
    {
        try
        {
            return measurePointBaseRepository.Delete(idMeasurePoint) != null;
        }
        catch (Exception ex) when (ex is EntityFetchingException ||
                                   ex is EntitySavingException)
        {
            return false;
        }
    }

    /// <summary>
    /// Modify MeasurePoint in database.
    /// </summary>
    /// <param name="id">Int representing MeasurePoint unique Id.</param>
    /// <param name="localId">Int representing MeasurePoint embded id.</param>
    /// <param name="name">String representing MeasurePoint name.</param>
    /// <param name="color">String representing MeasurePoint color.</param>
    /// <param name="unit">Int representing MeasurePoint unit.</param>
    /// <param name="componentId">Int representing measure point component holder id.</param>
    /// <returns></returns>
    /// <exception cref="EntityCreationException">Thrown if MeasurePoint informations doesn't meet requirements. (eg:color format)</exception>
    /// <exception cref="EntitySavingException">Thrown if updating doesn't execute properly.</exception>
    public MeasurePoint UpdateMeasurePoint (int id, 
                                            int localId, 
                                            string name, 
                                            string color, 
                                            int unit, 
                                            int componentId)
    {
        try
        {
            MeasurePoint MeasurePointToUpdate = new(localId, name, color, unit, componentId)
            {
                Id = id
            };
            return measurePointBaseRepository.Update(MeasurePointToUpdate);
        }
        catch (Exception ex) when(ex is InvalidMeasurePointLocalIdException ||
                                   ex is InvalidMeasurePointNameException ||
                                   ex is InvalidMeasurePointColorException ||
                                   ex is InvalidMeasurePointUnitException ||
                                   ex is InvalidMeasurePointComponentIdException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Retreive all MeasurePoint from Component.
    /// </summary>
    /// <param name="idComponent">Int representing Component unique Id that contains desired MeasurePoint.</param>
    /// <returns>List of MeasurePoint.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        return measurePointRepository.GetFromComponent(idComponent);
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
    public bool IsOwnerOfMeasurePoint(int userId, int idMeasurePoint)
    {
        try
        {
            var measurePoint = measurePointRepository.GetById(idMeasurePoint, true, true);
            return measurePoint.Component.Station.UserId == userId;
        }
        catch (EntityFetchingException)
        {
            return false;
        }
        
    }
}
