using SmartWeather.Entities.ComponentData;

namespace SmartWeather.Services.ComponentDatas;

public interface IMeasureDataRepository
{
    Task<IEnumerable<MeasureData>> GetFromMeasurePoint(int idMeasurePoint, DateTime? startPeriod = null, DateTime? endPeriod = null);

    void Create(MeasureData data);
}
