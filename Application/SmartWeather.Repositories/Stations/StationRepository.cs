using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Stations;

namespace SmartWeather.Repositories.Stations;

public class StationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IStationRepository
{
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

    public Station GetByMacAddress(string macAddress)
    {
        var stationsRetreived = readOnlyContext.Stations.Where(s => s.MacAddress == macAddress).FirstOrDefault();
        return stationsRetreived ?? throw new EntityFetchingException();
    }

    public IEnumerable<Station> GetFromUser(int userId)
    {
        var stationsRetreived = readOnlyContext.Stations.Where(s => s.UserId == userId).ToList();
        return stationsRetreived ?? throw new EntityFetchingException();
    }

    public bool IsOwnerOfStation(int userId, int idStation)
    {

        var station = readOnlyContext.Stations.Where(s => s.Id == idStation).FirstOrDefault();
        if (station == null) throw new EntityFetchingException();
        return station.UserId == userId;
    }
}
