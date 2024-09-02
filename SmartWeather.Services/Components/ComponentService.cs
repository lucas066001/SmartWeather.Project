using SmartWeather.Entities.Component;
using SmartWeather.Entities.User;
using SmartWeather.Services.Repositories;
using SmartWeather.Services.Users;

namespace SmartWeather.Services.Components;

public class ComponentService (IRepository<Component> componentBaseRepository)
{
    public Component AddNewComponent(string name, string color, ComponentUnit unit, ComponentType type, int stationId)
    {
        Component componentToCreate = new(name, color, unit, type, stationId);
        return componentBaseRepository.Create(componentToCreate);
    }
}
