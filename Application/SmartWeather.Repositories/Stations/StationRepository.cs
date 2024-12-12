namespace SmartWeather.Repositories.Stations;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

public class StationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IStationRepository
{
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoint)
    {
        IEnumerable<Station>? stationsRetreived = null;

        try
        {
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
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive stations in database : " + ex.Message);
        }

        return stationsRetreived;
    }

    public Station? GetByMacAddress(string macAddress)
    {
        Station? stationsRetreived = null;

        try
        {
            stationsRetreived = readOnlyContext.Stations.Where(s => s.MacAddress == macAddress).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive stations from user in database : " + ex.Message);
        }


        return stationsRetreived;
    }

    public IEnumerable<Station> GetFromUser(int userId)
    {
        IEnumerable<Station> stationsRetreived = null!;

        try
        {
            stationsRetreived = readOnlyContext.Stations.Where(s => s.UserId == userId).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive stations from user in database : " + ex.Message);
        }

        return stationsRetreived;
    }

    public bool IsOwnerOfStation(int userId, int idStation)
    {
        try
        {
            return readOnlyContext.Stations.Where(s => s.Id == idStation).FirstOrDefault()?.UserId == userId;
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive stations from database : " + ex.Message);
        }
    }
}
