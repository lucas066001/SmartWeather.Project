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
    /// Retreive all Stations from table.
    /// Possibility to include components and measure points per station.
    /// </summary>
    /// <param name="includeComponents">Bool indicating to include or not station's components.</param>
    /// <param name="includeMeasurePoints">Bool indicating to include or not station's measure points.</param>
    /// <returns>List of Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoints);
}
