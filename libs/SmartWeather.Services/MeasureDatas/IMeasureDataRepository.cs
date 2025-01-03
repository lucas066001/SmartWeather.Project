namespace SmartWeather.Services.ComponentDatas;

using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.Common;

public interface IMeasureDataRepository
{
    /// <summary>
    /// Retreive MeasureData from specific point within desired period.
    /// </summary>
    /// <param name="idMeasurePoint">Int representing MeasurePoint unique Id.</param>
    /// <param name="startPeriod">DateTime representing start time period.</param>
    /// <param name="endPeriod">DateTime representing end time period.</param>
    /// <returns>Result containing list of MeasureData.</returns>
    Task<Result<IEnumerable<MeasureData>>> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod);

    /// <summary>
    /// Insert MeasureData into database.
    /// </summary>
    /// <param name="data">MeasureData object to be inserted.</param>
    void Create(MeasureData data);
}
