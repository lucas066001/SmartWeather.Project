namespace SmartWeather.Api.Controllers.Component.Dtos.Converters;

using SmartWeather.Entities.Component;

public class ComponentListResponseConverter
{
    public static ComponentListResponse ConvertComponentListToComponentListResponse(IEnumerable<Component> components)
    {
        ComponentListResponse result = new ComponentListResponse()
        {
            ComponentList = new List<ComponentResponse>()
        };

        foreach (var component in components)
        {
            result.ComponentList.Add(ComponentResponseConverter.ConvertComponentToComponentResponse(component));
        }
        return result;
    }

    public static List<ComponentResponse> ConvertComponentListToComponentList(IEnumerable<Component> components)
    {
        var result = new List<ComponentResponse>();

        foreach (var component in components)
        {
            result.Add(ComponentResponseConverter.ConvertComponentToComponentResponse(component));
        }
        return result;
    }
}
