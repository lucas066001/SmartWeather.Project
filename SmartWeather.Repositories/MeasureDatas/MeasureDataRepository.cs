using SmartWeather.Entities.ComponentData;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.ComponentDatas;

namespace SmartWeather.Repositories.ComponentDatas;

public class MeasureDataRepository(Func<SmartWeatherReadOnlyContext> readOnlyContextFactory) : IMeasureDataRepository
{
    public IEnumerable<MeasureData> GetFromMeasurePoint(int idMeasurePoint, DateTime? startPeriod = null, DateTime? endPeriod = null)
    {
        IEnumerable<MeasureData> componentDatasRetreived = null!;
        using (var roContext = readOnlyContextFactory())
        {
            try
            {
                if (startPeriod != null && endPeriod != null)
                {
                    componentDatasRetreived = roContext.MeasureDatas.Where(cd => cd.MeasurePointId == idMeasurePoint && (cd.DateTime >= startPeriod && cd.DateTime <= endPeriod)).ToList();
                }
                else
                {
                    componentDatasRetreived = roContext.MeasureDatas.Where(cd => cd.MeasurePointId == idMeasurePoint).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retreive components from station in database : " + ex.Message);
            }
        }

        return componentDatasRetreived;
    }
}
