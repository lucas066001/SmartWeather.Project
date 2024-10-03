namespace SmartWeather.Repositories.Components;

using SmartWeather.Entities.Component;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Components;
using System;
using System.Collections.Generic;

internal class ComponentRepository(Func<SmartWeatherReadOnlyContext> readOnlyContextFactory) : IComponentRepository
{
    public IEnumerable<Component> GetFromStation(int stationId)
    {
        IEnumerable<Component> componentsRetreived = null!;
        using (var roContext = readOnlyContextFactory())
        {
            try
            {
                componentsRetreived = roContext.Components.Where(s => s.StationId == stationId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retreive components from station in database : " + ex.Message);
            }
        }

        return componentsRetreived;
    }
}
