using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Station;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Stations;

namespace SmartWeather.Repositories.Stations;

public class StationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IStationRepository
{
    /// <summary>
    /// Retreive all Stations from table.
    /// Possibility to include components and measure points per station.
    /// </summary>
    /// <param name="includeComponents">Bool indicating to include or not station's components.</param>
    /// <param name="includeMeasurePoint">Bool indicating to include or not station's measure points.</param>
    /// <returns>List of Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoint)
    {
        IEnumerable<Station>? stationsRetreived = null;

        if (includeMeasurePoint) 
        {
            stationsRetreived = readOnlyContext.Stations
                                .Include(s => s.Components)
                                .ThenInclude(c => c.MeasurePoints)
                                .AsEnumerable();
        }
        else if (includeComponents)
        {
            stationsRetreived = readOnlyContext.Stations
                                .Include(s => s.Components)
                                .AsEnumerable();
        }
        else
        {
            stationsRetreived = readOnlyContext.Stations.AsEnumerable();
        }

        return stationsRetreived ?? throw new EntityFetchingException();
    }

    /// <summary>
    /// Retreive a Station based on given mac address.
    /// </summary>
    /// <param name="macAddress">Mac address owned by desired station.</param>
    /// <returns>A Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public Station GetByMacAddress(string macAddress)
    {
        var stationsRetreived = readOnlyContext.Stations.Where(s => s.MacAddress == macAddress).FirstOrDefault();
        return stationsRetreived ?? throw new EntityFetchingException();
    }

    /// <summary>
    /// Retreive all Stations owned by a User.
    /// </summary>
    /// <param name="userId">Int representing unique Id of Station owner.</param>
    /// <returns>A list of Station.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Station> GetFromUser(int userId)
    {
        var stationsRetreived = readOnlyContext.Stations.Where(s => s.UserId == userId).ToList();
        return stationsRetreived ?? throw new EntityFetchingException();
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
    /// <exception cref="EntityFetchingException">Thrown if no station can be found.</exception>
    public bool IsOwnerOfStation(int userId, int idStation)
    {

        var station = readOnlyContext.Stations.Where(s => s.Id == idStation).FirstOrDefault();
        if (station == null) throw new EntityFetchingException();
        return station.UserId == userId;
    }
}
