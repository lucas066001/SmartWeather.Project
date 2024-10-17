namespace SmartWeather.Api.Controllers.ComponentData.Dtos.Converters;

using SmartWeather.Entities.ComponentData;

public class MeasureDataResponseConverter
{
    public static MeasureDataResponse ConvertComponentDataToComponentDataResponse(MeasureData measureData)
    {
        return new MeasureDataResponse()
        {
            MeasurePointId = measureData.MeasurePointId,
            Value = measureData.Value,
            DateTime = measureData.DateTime
        };
    }
}
