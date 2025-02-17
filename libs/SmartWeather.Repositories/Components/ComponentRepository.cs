namespace SmartWeather.Repositories.Components;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Components;
using System.Collections.Generic;
using SmartWeather.Entities.Common;

public class ComponentRepository(SmartWeatherReadOnlyContext readOnlyContext) : IComponentRepository
{
    public Result<Component> GetById(int componentId, bool includeStation)
    {
        Component? componentRetreived = null;

        if (includeStation)
        {
            componentRetreived = readOnlyContext.Components
                                                     .Include(c => c.Station)
                                                     .Where(c => c.Id == componentId)
                                                     .FirstOrDefault();
        }
        else
        {
            componentRetreived = readOnlyContext.Components
                                                     .Where(c => c.Id == componentId)
                                                     .FirstOrDefault();
        }

        return componentRetreived != null ?
                                        Result<Component>.Success(componentRetreived) :
                                        Result<Component>.Failure(string.Format(
                                                                        ExceptionsBaseMessages.ENTITY_FETCH,
                                                                        nameof(Component)));
    }

    public Result<IEnumerable<Component>> GetFromStation(int stationId, bool includeComponents)
    {
        var componentsRetreived = new List<Component>();

        if (!includeComponents)
        {
            componentsRetreived = readOnlyContext.Components.Where(s => s.StationId == stationId).ToList();
        }
        else
        {
            componentsRetreived = readOnlyContext.Components.Include(c => c.MeasurePoints).Where(s => s.StationId == stationId).ToList();
        }

        return componentsRetreived != null ?
                    Result<IEnumerable<Component>>.Success(componentsRetreived) :
                    Result<IEnumerable<Component>>.Failure(string.Format(
                                                            ExceptionsBaseMessages.ENTITY_FETCH,
                                                            nameof(Component)));
    }
}
