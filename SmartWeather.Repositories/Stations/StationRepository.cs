namespace SmartWeather.Repositories.Stations;

using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

public class StationRepository(Func<SmartWeatherReadOnlyContext> contextFactory) : IStationRepository
{
    public Station? GetByMacAddress(string macAddress)
    {
        Station? stationsRetreived = null;
        using (var roContext = contextFactory())
        {
            try
            {
                stationsRetreived = roContext.Stations.Where(s => s.MacAddress == macAddress).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retreive stations from user in database : " + ex.Message);
            }
        }

        return stationsRetreived;
    }

    public IEnumerable<Station> GetFromUser(int userId)
    {
        IEnumerable<Station> stationsRetreived = null!;
        using (var roContext = contextFactory())
        {
            try
            {
                stationsRetreived = roContext.Stations.Where(s => s.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retreive stations from user in database : " + ex.Message);
            }
        }

        return stationsRetreived;
    }
}
