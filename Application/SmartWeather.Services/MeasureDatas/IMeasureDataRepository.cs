using SmartWeather.Entities.ComponentData;

namespace SmartWeather.Services.ComponentDatas;

public interface IMeasureDataRepository
{
    /// <summary>
    /// Insert MeasureData into database/
    /// </summary>
    /// <param name="data">MeasureData object to be inserted.</param>
    /// <exception cref="EntitySavingException">Thrown if error occurs during saving.</exception>
    Task<IEnumerable<MeasureData>> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod);

    /// <summary>
    /// Retreive MeasureData within desired period.
    /// </summary>
    /// <param name="idMeasurePoint">Int representing MeasurePoint unique Id.</param>
    /// <param name="startPeriod">DateTime representing start time period.</param>
    /// <param name="endPeriod">DateTime representing end time period.</param>
    /// <returns>List of MeasureData.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data can be read within requested params.</exception>
    void Create(MeasureData data);
}
