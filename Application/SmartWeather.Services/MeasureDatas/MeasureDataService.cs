namespace SmartWeather.Services.ComponentDatas;

using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.Common.Exceptions;

public class MeasureDataService (IMeasureDataRepository measureDataRepository)
{
    /// <summary>
    /// Insert MeasureData into database.
    /// </summary>
    /// <param name="measurePointId">Int representing measure point id that produced the data.</param>
    /// <param name="value">Float representing the value acquired by the measure point.</param>
    /// <param name="dateTime">DateTime representing the acquire time of data.</param>
    /// <exception cref="EntitySavingException">Thrown if error occurs during saving.</exception>
    public void InsertMeasureData(int measurePointId, float value, DateTime dateTime)
    {
        MeasureData ComponentDataToCreate = new(measurePointId, value, dateTime);
        measureDataRepository.Create(ComponentDataToCreate);
    }

    /// <summary>
    /// Retreive MeasureData from specific point within desired period.
    /// </summary>
    /// <param name="idMeasurePoint">Int representing MeasurePoint unique Id.</param>
    /// <param name="startPeriod">DateTime representing start time period.</param>
    /// <param name="endPeriod">DateTime representing end time period.</param>
    /// <returns>List of MeasureData.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data can be read within requested params.</exception>
    public IEnumerable<MeasureData> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod)
    {
        return measureDataRepository.GetFromMeasurePoint(idMeasurePoint, startPeriod, endPeriod).Result;
    }
}
