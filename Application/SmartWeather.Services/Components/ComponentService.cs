using SmartWeather.Entities.Component;
using SmartWeather.Services.Repositories;

namespace SmartWeather.Services.Components;

public class ComponentService (IRepository<Component> componentBaseRepository, IComponentRepository componentRepository)
{
    public Component AddNewComponent(string name, string color, int type, int stationId, int gpioPin)
    {
        Component componentToCreate = new(name, color, type, stationId, gpioPin);
        return componentBaseRepository.Create(componentToCreate);
    }
    public IEnumerable<Component> AddGenericComponentPool(int stationId, IEnumerable<int> gpioPins)
    {
        var createdComponents = new List<Component>();
        foreach (var pin in gpioPins)
        {
            Component componentToCreate = new("Unnamed component", "#000000", (int)ComponentType.Unknown, stationId, pin);
            componentBaseRepository.Create(componentToCreate);
            createdComponents.Add(componentToCreate);
        }
        return createdComponents;
    }

    public bool DeleteComponent(int idComponent)
    {
        return componentBaseRepository.Delete(idComponent) != null;
    }

    public Component UpdateComponent(int id, string name, string color, int type, int stationId, int gpioPin)
    {
        Component componentToUpdate = new(name, color, type, stationId, gpioPin)
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
