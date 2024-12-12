namespace SmartWeather.Api.Controllers.Component.Dtos.Converters;

using SmartWeather.Entities.Component;
using SmartWeather.Api.Controllers.MeasurePoint.Dtos.Converters;

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
            MeasurePoints = component.MeasurePoints.Any() ? MeasurePointConverter.ConvertMeasurePointListToMeasurePointList(component.MeasurePoints) : null
        };
    }
}
