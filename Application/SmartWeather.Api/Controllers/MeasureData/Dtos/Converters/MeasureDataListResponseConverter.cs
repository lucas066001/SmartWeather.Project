namespace SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;

using SmartWeather.Api.Controllers.User.Dtos;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.User;

public class MeasureDataListResponseConverter
{
    public static MeasureDataListResponse ConvertComponentDataListToComponentDataListResponse(IEnumerable<MeasureData> measureDatas)
    {
        MeasureDataListResponse response = new MeasureDataListResponse() { MeasureDataList = new List<MeasureDataResponse>() };

        foreach (var measureData in measureDatas)
        {
            response.MeasureDataList.Add(MeasureDataResponseConverter.ConvertComponentDataToComponentDataResponse(measureData));
        }
        return response;
    }
}
