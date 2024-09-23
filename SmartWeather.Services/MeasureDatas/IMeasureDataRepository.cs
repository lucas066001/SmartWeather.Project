using SmartWeather.Entities.ComponentData;

namespace SmartWeather.Services.ComponentDatas;

public interface IMeasureDataRepository
{
    IEnumerable<MeasureData> GetFromMeasurePoint(int idMeasurePoint, DateTime? startPeriod = null, DateTime? endPeriod = null);
}
