using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.MeasurePoints;

namespace SmartWeather.Repositories.MeasurePoints;

public class MeasurePointRepository(Func<SmartWeatherReadOnlyContext> contextFactory) : IMeasurePointRepository
{
    public IEnumerable<MeasurePoint> GetFromComponent(int idComponent)
    {
        IEnumerable<MeasurePoint> measurePointsRetreived = null!;
        using (var roContext = contextFactory())
        {
            try
            {
                measurePointsRetreived = roContext.MeasurePoints.Where(cd => cd.ComponentId == idComponent).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retreive measurePoints from component in database : " + ex.Message);
            }
        }

        return measurePointsRetreived;
    }
}
