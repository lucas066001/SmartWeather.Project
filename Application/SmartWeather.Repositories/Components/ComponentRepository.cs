namespace SmartWeather.Repositories.Components;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Components;
using System.Collections.Generic;

public class ComponentRepository(SmartWeatherReadOnlyContext readOnlyContext) : IComponentRepository
{
    public Component GetById(int componentId, bool includeStation)
    {
        if (includeStation)
        {
            var componentsRetreived = readOnlyContext.Components
                                                     .Include(c => c.Station)
                                                     .Where(c => c.Id == componentId).FirstOrDefault();

            return componentsRetreived ?? throw new EntityFetchingException();
        }
        else
        {
            var componentsRetreived = readOnlyContext.Components
                                         .Where(c => c.Id == componentId).FirstOrDefault();

            return componentsRetreived ?? throw new EntityFetchingException();
        }
    }

    public IEnumerable<Component> GetFromStation(int stationId)
    {
        var componentsRetreived = readOnlyContext.Components.Where(s => s.StationId == stationId).ToList();

        return componentsRetreived ?? throw new EntityFetchingException();
    }

    public bool IsOwnerOfComponent(int userId, int idComponent)
    {
        var component = readOnlyContext.Components.Where(s => s.Id == idComponent)
            .Include(c => c.Station)
            .FirstOrDefault();

        if(component == null) throw new EntityFetchingException();

        return component.Station.UserId == userId;
    }
}
