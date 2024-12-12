namespace SmartWeather.Repositories.Components;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Components;
using System.Collections.Generic;

public class ComponentRepository(SmartWeatherReadOnlyContext readOnlyContext) : IComponentRepository
{
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
