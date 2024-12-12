namespace SmartWeather.Repositories.Components;

using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Component;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Components;
using System;
using System.Collections.Generic;

public class ComponentRepository(SmartWeatherReadOnlyContext readOnlyContext) : IComponentRepository
{
    /// <summary>
    /// Retreive Components from a given Station.
    /// </summary>
    /// <param name="stationId">Int representing Station unique Id.</param>
    /// <returns>List of Component.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<Component> GetFromStation(int stationId)
    {
        var componentsRetreived = readOnlyContext.Components.Where(s => s.StationId == stationId).ToList();

        return componentsRetreived ?? throw new EntityFetchingException();
    }

    /// <summary>
    /// Check wether or not a User owns a Component.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="idComponent">Int representing Component unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the Component.
    /// (True : Owner | False : Not owner)
    /// </returns>
    /// <exception cref="Exception"></exception>
    public bool IsOwnerOfComponent(int userId, int idComponent)
    {
        var component = readOnlyContext.Components.Where(s => s.Id == idComponent)
            .Include(c => c.Station)
            .FirstOrDefault();

        if(component == null) throw new EntityFetchingException();

        return component.Station.UserId == userId;
    }
}
