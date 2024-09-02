using SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Services.Repositories;
using SmartWeather.Services.Users;

namespace SmartWeather.Services.Components;

public class ComponentService (IRepository<Component> componentBaseRepository, IComponentRepository componentRepository)
{
    public Component AddNewComponent(string name, string color, int unit, int type, int stationId)
    {
        Component componentToCreate = new(name, color, unit, type, stationId);
        return componentBaseRepository.Create(componentToCreate);
    }

    public bool DeleteComponent(int idComponent)
    {
        return componentBaseRepository.Delete(idComponent) != null;
    }

    public Component UpdateComponent(int id, string name, string color, int unit, int type, int stationId)
    {
        Component componentToUpdate = new(name, color, unit, type, stationId)
        {
            Id = id
        };
        return componentBaseRepository.Update(componentToUpdate);
    }

    public IEnumerable<Component> GetFromStation(int idStation)
    {
        return componentRepository.GetFromStation(idStation);
    }

    public Component GetById(int componentId)
    {
        return componentBaseRepository.GetById(componentId);
    }
}
