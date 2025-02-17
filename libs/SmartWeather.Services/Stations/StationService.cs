namespace SmartWeather.Services.Stations;

using SmartWeather.Entities.Station;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Services.Repositories;
using SmartWeather.Entities.Station.Exceptions;
using SmartWeather.Entities.Common;

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
    /// <returns>Result containing created Station from database, including auto-generated fields (e.g: id).</returns>
    public Result<Station> AddNewStation(string name, string macAddress, float latitude, float longitude, int? userId)
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
            return Result<Station>.Failure(string.Format(
                                                    ExceptionsBaseMessages.ENTITY_FORMAT,
                                                    nameof(Station), 
                                                    ex.Message));    
        }
    }

    /// <summary>
    /// Create a Station entity in database using given mac address and sample values.
    /// Mainly used by Station configuration automatic process.
    /// </summary>
    /// <param name="macAddress">String representing station mac address.</param>
    /// <returns>Result containing created Station from database.</returns>
    public Result<Station> AddGenericStation(string macAddress)
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
        return stationBaseRepository.Delete(idStation).IsSuccess;
    }

    /// <summary>
    /// Modify entity in database.
    /// </summary>
    /// <param name="id">Int representing Station unique Id.</param>
    /// <param name="name">String representing Station name.</param>
    /// <param name="latitude">Float representing Station latitude coordinate.</param>
    /// <param name="longitude">Float representing Station longitude coordinate.</param>
    /// <param name="userId">Nullable Int representing Station owner idn could be null if no current owner.</param>
    /// <returns>Result containing modified Station from database.</returns>
    public Result<Station> UpdateStation (int id, 
                                            string name, 
                                            float latitude, 
                                            float longitude, 
                                            int? userId)
    {
        try
        {
            var stationToUpdate = stationBaseRepository.GetById(id);
            if (stationToUpdate.IsSuccess)
            {
                stationToUpdate.Value.Name = name;
                stationToUpdate.Value.Latitude = latitude;
                stationToUpdate.Value.Longitude = longitude;
                stationToUpdate.Value.UserId = userId;
                return stationBaseRepository.Update(stationToUpdate.Value);

            }
            else
            {
                return Result<Station>.Failure(string.Format(
                                        ExceptionsBaseMessages.ENTITY_FETCH,
                                        nameof(Station)));
            }
        }
        catch (Exception ex) when (ex is InvalidStationNameException ||
                                   ex is InvalidStationMacAddressException ||
                                   ex is InvalidStationCoordinatesException ||
                                   ex is InvalidStationUserIdException)
        {
            return Result<Station>.Failure(string.Format(
                                                    ExceptionsBaseMessages.ENTITY_FORMAT,
                                                    nameof(Station),
                                                    ex.Message));
        }

    }

    /// <summary>
    /// Retreive Station based on it's Id.
    /// </summary>
    /// <param name="idStation">Int corresponding to Station unique Id.</param>
    /// <returns>Result containing selected Station.</returns>
    public Result<Station> GetStationById(int idStation)
    {
        return stationBaseRepository.GetById(idStation);
    }

    /// <summary>
    /// Retreive a Station based on given mac address.
    /// </summary>
    /// <param name="macAddress">Mac address owned by desired station.</param>
    /// <returns>Result containing selected Station.</returns>
    public Result<Station> GetStationByMacAddress(string macAddress)
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
        return stationRepository.GetByMacAddress(macAddress).IsSuccess;
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
        var station = stationBaseRepository.GetById(idStation);
        return station.IsSuccess && station.Value.UserId == userId;
    }

    /// <summary>
    /// Retreive all Stations from table.
    /// Possibility to include components and measure points per station.
    /// </summary>
    /// <param name="includeComponents">Bool indicating to include or not station's components.</param>
    /// <param name="includeMeasurePoints">Bool indicating to include or not station's measure points.</param>
    /// <returns>Result contining a list of Stations.</returns>
    public Result<IEnumerable<Station>> GetAll(bool includeComponents, bool includeMeasurePoints)
    {
        return stationRepository.GetAll(includeComponents, includeMeasurePoints);
    }

    /// <summary>
    /// Get all Stations from a User identified with it's unique Id.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <returns>Result containing the list of Station owned by the user.</returns>
    public Result<IEnumerable<Station>> GetFromUser(int userId) 
    {
        return stationRepository.GetFromUser(userId);
    }
}
