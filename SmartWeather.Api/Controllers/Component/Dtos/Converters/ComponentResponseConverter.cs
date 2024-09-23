namespace SmartWeather.Api.Controllers.Component.Dtos.Converters;

using SmartWeather.Api.Controllers.Station.Dtos.Converters;
using SmartWeather.Api.Controllers.Station.Dtos;
using SmartWeather.Entities.Component;

public class ComponentResponseConverter
{
    public static ComponentResponse ConvertComponentToComponentResponse(Component component)
    {
        return new ComponentResponse() { 
            Id = component.Id,
            GpioPin = component.GpioPin,
            Color = component.Color,
            Name = component.Name,
            StationId = component.StationId,
            Type = component.Type,  
        };
    }
}
