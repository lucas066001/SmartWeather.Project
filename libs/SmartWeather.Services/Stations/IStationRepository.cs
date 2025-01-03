namespace SmartWeather.Services.Stations;

using SmartWeather.Entities.Station;
using System.Collections.Generic;
using SmartWeather.Entities.Common;

public interface IStationRepository
{
    /// <summary>
    /// Retreive all Stations owned by a User.
    /// </summary>
    /// <param name="userId">Int representing unique Id of Station owner.</param>
    /// <returns>Result containing a list of Station.</returns>
    public Result<IEnumerable<Station>> GetFromUser(int userId);

    /// <summary>
    /// Retreive a Station based on given mac address.
    /// </summary>
    /// <param name="macAddress">Mac address owned by desired station.</param>
    /// <returns>Result containing a Station.</returns>
    public Result<Station> GetByMacAddress(string macAddress);

    /// <summary>
    /// Retreive all Stations from table.
    /// Possibility to include components and measure points per station.
    /// </summary>
    /// <param name="includeComponents">Bool indicating to include or not station's components.</param>
    /// <param name="includeMeasurePoints">Bool indicating to include or not station's measure points.</param>
    /// <returns>Result containing a list of Station.</returns>
    public Result<IEnumerable<Station>> GetAll(bool includeComponents, bool includeMeasurePoints);
}
