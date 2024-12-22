namespace SmartWeather.Services.Stations;

using SmartWeather.Entities.Station;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Services.Repositories;
using SmartWeather.Entities.Station.Exceptions;
using SmartWeather.Services.Components;

public class StationService(IRepository<Station> stationBaseRepository, IStationRepository stationRepository)
{
    /// <summary>
    /// Create a Station in database based on given infos.
    /// </summary>
    /// <param name="name">String representing station name.</param>
    /// <param name="macAddress">String representing station mac address.</param>
    /// <param name="latitude">Float representing station latitude coordinate.</param>
    /// <param name="longitude">Float representing station longitude coordinate.</param>
    /// <param name="userId">Nullable Int representing station owner idn could be null if no current owner.</param>
    /// <returns>Created Station from database.</returns>
    /// <exception cref="EntityCreationException">Thrown if Station informations doesn't meet requirements. (eg:mac address format)</exception>
    /// <exception cref="EntitySavingException">Thrown if updating doesn't execute properly.</exception>
    public Station AddNewStation(string name, string macAddress, float latitude, float longitude, int? userId)
    {
        try
        {
            Station stationToCreate = new(name, macAddress, latitude, longitude, userId);
            return stationBaseRepository.Create(stationToCreate);
        }
        catch (Exception ex) when (ex is InvalidStationNameException ||
                                   ex is InvalidStationMacAddressException ||
                                   ex is InvalidStationCoordinatesException ||
                                   ex is InvalidStationUserIdException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Create a Station entity in database using given mac address and sample values.
    /// Mainly used by Station configuration automatic process.
    /// </summary>
    /// <param name="macAddress">String representing station mac address.</param>
    /// <returns>Created Station from database.</returns>
    /// <exception cref="EntityCreationException">Thrown if Station informations doesn't meet requirements. (eg:mac address format)</exception>
    /// <exception cref="EntitySavingException">Thrown if updating doesn't execute properly.</exception>
    public Station AddGenericStation(string macAddress)
    {
        return AddNewStation("Unnamed Station", macAddress, 0.0f, 0.0f, null);
    }

    /// <summary>
    /// Delete Station based on given id.
    /// </summary>
    /// <param name="entityId">Int that correspond to Station unique Id.</param>
    /// <returns>
    /// Bool indicating deletion MeasurePoint success.
    /// (True : Success | False : Failure)
    /// </returns>
    public bool DeleteStation(int idStation)
    {
        try
        {
            return stationBaseRepository.Delete(idStation) != null;
        }
        catch (Exception ex) when (ex is EntityFetchingException ||
                                   ex is EntitySavingException)
        {
            return false;
        }
    }

    /// <summary>
    /// Modify entity in database.
    /// </summary>
    /// <param name="id">Int representing Station unique Id.</param>
    /// <param name="name">String representing Station name.</param>
    /// <param name="macAddress">String representing Station mac address.</param>
    /// <param name="latitude">Float representing Station latitude coordinate.</param>
    /// <param name="longitude">Float representing Station longitude coordinate.</param>
    /// <param name="userId">Nullable Int representing Station owner idn could be null if no current owner.</param>
    /// <returns>Modified Station from database.</returns>
    /// <exception cref="EntityCreationException">Thrown if Station informations doesn't meet requirements. (eg:mac address format)</exception>
    /// <exception cref="EntitySavingException">Thrown if updating doesn't execute properly.</exception>
    public Station UpdateStation (int id, 
                                  string name, 
                                  string macAddress, 
                                  float latitude, 
                                  float longitude, 
                                  int? userId)
    {
        try
        {
            Station stationToUpdate = new(name, macAddress, latitude, longitude, userId)
            {
                Id = id
            };
            return stationBaseRepository.Update(stationToUpdate);
        }
        catch (Exception ex) when (ex is InvalidStationNameException ||
                                   ex is InvalidStationMacAddressException ||
                                   ex is InvalidStationCoordinatesException ||
                                   ex is InvalidStationUserIdException)
        {
            throw new EntityCreationException();
        }

    }

    /// <summary>
    /// Retreive Station based on it's Id.
    /// </summary>
    /// <param name="idStation">Int corresponding to Station unique Id.</param>
    /// <returns>A Station object.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public Station GetStationById(int idStation)
    {
        return stationBaseRepository.GetById(idStation);
    }

    /// <summary>
    /// Retreive a Station based on given mac address.
    /// </summary>
    /// <param name="macAddress">Mac address owned by desired station.</param>
    /// <returns>A Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public Station GetStationByMacAddress(string macAddress)
    {
        return stationRepository.GetByMacAddress(macAddress);
    }

    /// <summary>
    /// Check wether or not a Station with the same mac address exist.
    /// </summary>
    /// <param name="macAddress">String representing mac address to check.</param>
    /// <returns>
    /// A boolean representing if Mac Address already in use.
    /// (True : Used | False : Unused)
    /// </returns>
    public bool IsStationRegistered(string macAddress)
    {
        try
        {
            _ = stationRepository.GetByMacAddress(macAddress);
            return true;
        }
        catch (EntityFetchingException)
        {
            return false;
        }
    }

    /// <summary>
    /// Check wether or not a user owns a station.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="idStation">Int representing Station unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the Station.
    /// (True : Owner | False : Not owner)
    /// </returns>
    public bool IsOwnerOfStation(int userId, int idStation)
    {
        try
        {
            var station = stationBaseRepository.GetById(idStation);
            return station.UserId == userId;
        }
        catch (EntityFetchingException)
        {
            return false;
        }
    }

    /// <summary>
    /// Retreive all Stations from table.
    /// Possibility to include components and measure points per station.
    /// </summary>
    /// <param name="includeComponents">Bool indicating to include or not station's components.</param>
    /// <param name="includeMeasurePoints">Bool indicating to include or not station's measure points.</param>
    /// <returns>List of Stations.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoints)
    {
        return stationRepository.GetAll(includeComponents, includeMeasurePoints);
    }

    /// <summary>
    /// Get all Stations from a User identified with it's unique Id.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <returns>List of Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetFromUser(int userId) 
    {
        return stationRepository.GetFromUser(userId);
    }
}
