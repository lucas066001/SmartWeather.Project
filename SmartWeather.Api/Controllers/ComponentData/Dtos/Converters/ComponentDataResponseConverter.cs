namespace SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;

using SmartWeather.Entities.ComponentData;

public class ComponentDataResponseConverter
{
    public static ComponentDataResponse ConvertComponentDataToComponentDataResponse(ComponentData componentData)
    {
        return new ComponentDataResponse()
        {
            ComponentId = componentData.ComponentId,
            Value = componentData.Value,
            DateTime = componentData.DateTime
        };
    }
}
