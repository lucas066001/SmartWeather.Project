﻿namespace SmartWeather.Repositories.Stations;

using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

public class StationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IStationRepository
{
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
