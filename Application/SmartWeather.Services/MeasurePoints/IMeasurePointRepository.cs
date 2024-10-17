using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.MeasurePoint;

namespace SmartWeather.Services.MeasurePoints;

public interface IMeasurePointRepository
{
    IEnumerable<MeasurePoint> GetFromComponent(int idComponent);
}
