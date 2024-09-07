using SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartWeather.Services.Mqtt.Dtos.StationConfigResponse;

namespace SmartWeather.Services.Mqtt.Dtos.Converters;

public class StationConfigResponseConverter
{
    public static StationConfigResponse ConvertStationToStationConfigResponse(Station station)
    {
        return new(station.Id, ConvertListComponentToListComponentConfig(station.Components));
    }

    public static IEnumerable<ComponentConfig> ConvertListComponentToListComponentConfig(IEnumerable<Component> components)
    {
        var result = new List<ComponentConfig>();

        foreach (var component in components)
        {
            result.Add(ConvertComponentToComponentConfig(component));
        }
        return result;
    }

    public static ComponentConfig ConvertComponentToComponentConfig(Component component)
    {
        return new ComponentConfig()
        {
            ComponentDatabaseId = component.Id,
            GpioPin = component.GpioPin
        };
    }
}
