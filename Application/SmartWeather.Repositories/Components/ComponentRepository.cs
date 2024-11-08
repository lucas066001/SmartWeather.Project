namespace SmartWeather.Repositories.Components;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Components;
using System;
using System.Collections.Generic;

internal class ComponentRepository(SmartWeatherReadOnlyContext readOnlyContext) : IComponentRepository
{
    public IEnumerable<Component> GetFromStation(int stationId)
    {
        IEnumerable<Component> componentsRetreived = null!;
        try
        {
            componentsRetreived = readOnlyContext.Components.Where(s => s.StationId == stationId).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive components from station in database : " + ex.Message);
        }

        return componentsRetreived;
    }

    public bool IsOwnerOfComponent(int userId, int idComponent)
    {
        try
        {
            return readOnlyContext.Components.Where(s => s.Id == idComponent)
                .Include(c => c.Station)
                .FirstOrDefault()?
                .Station.UserId == userId;
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive components from database : " + ex.Message);
        }
    }
}
