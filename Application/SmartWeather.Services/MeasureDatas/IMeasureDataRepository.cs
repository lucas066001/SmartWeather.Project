using SmartWeather.Entities.ComponentData;

namespace SmartWeather.Services.ComponentDatas;

public interface IMeasureDataRepository
{
    Task<IEnumerable<MeasureData>> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod);

    void Create(MeasureData data);
}
