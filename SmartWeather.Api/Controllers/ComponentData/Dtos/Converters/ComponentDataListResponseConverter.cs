namespace SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;

using SmartWeather.Api.Controllers.User.Dtos;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.User;

public class ComponentDataListResponseConverter
{
    public static ComponentDataListResponse ConvertComponentDataListToComponentDataListResponse(IEnumerable<ComponentData> componentDatas)
    {
        ComponentDataListResponse response = new ComponentDataListResponse() { ComponentDataList = new List<ComponentDataResponse>() };

        foreach (var componentData in componentDatas)
        {
            response.ComponentDataList.Add(ComponentDataResponseConverter.ConvertComponentDataToComponentDataResponse(componentData));
        }
        return response;
    }
}
