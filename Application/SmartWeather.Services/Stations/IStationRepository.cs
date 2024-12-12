namespace SmartWeather.Services.Stations;

using SmartWeather.Entities.Station;
using SmartWeather.Entities.Common.Exceptions;
using System.Collections.Generic;

public interface IStationRepository
{
    /// <summary>
    /// Retreive all Stations owned by a User.
    /// </summary>
    /// <param name="userId">Int representing unique Id of Station owner.</param>
    /// <returns>A list of Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetFromUser(int userId);

    /// <summary>
    /// Retreive a Station based on given mac address.
    /// </summary>
    /// <param name="macAddress">Mac address owned by desired station.</param>
    /// <returns>A Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public Station GetByMacAddress(string macAddress);

    /// <summary>
    /// Check wether or not a user owns a station.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="idStation">Int representing Station unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the Station.
    /// (True : Owner | False : Not owner)
    /// </returns>
    /// <exception cref="EntityFetchingException">Thrown if no station can be found.</exception>
    public bool IsOwnerOfStation(int userId, int idStation);

    /// <summary>
    /// Retreive all Stations from table.
    /// Possibility to include components and measure points per station.
    /// </summary>
    /// <param name="includeComponents">Bool indicating to include or not station's components.</param>
    /// <param name="includeMeasurePoint">Bool indicating to include or not station's measure points.</param>
    /// <returns>List of Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoint);
}
