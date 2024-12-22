using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Stations;

namespace SmartWeather.Repositories.Stations;

public class StationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IStationRepository
{
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoints)
    {
        IEnumerable<Station>? stationsRetreived = null;

        if (includeMeasurePoints) 
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

    public Station GetByMacAddress(string macAddress)
    {
        var stationRetreived = readOnlyContext.Stations.Where(s => s.MacAddress == macAddress).FirstOrDefault();
        return stationRetreived ?? throw new EntityFetchingException();
    }

    public IEnumerable<Station> GetFromUser(int userId)
    {
        var stationsRetreived = readOnlyContext.Stations.Where(s => s.UserId == userId).ToList();
        return stationsRetreived ?? throw new EntityFetchingException();
    }
}
