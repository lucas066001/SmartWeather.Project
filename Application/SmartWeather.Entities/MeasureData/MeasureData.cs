﻿namespace SmartWeather.Entities.ComponentData;

public class MeasureData
{
    public int MeasurePointId { get; set; }
    public float Value { get; set; }
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Create a MeasureData based on given informations.
    /// (Must kept really light because expose to high frequence calls)
    /// </summary>
    /// <param name="measurePointId">Int representing measure point id that produced the data.</param>
    /// <param name="value">Float representing the value acquired by the measure point.</param>
    /// <param name="dateTime">DateTime representing the acquire time of data.</param>
    /// <returns>A MeasureData object.</returns>
    public MeasureData(int measurePointId, float value, DateTime dateTime)
    {
        MeasurePointId = measurePointId;
        Value = value;
        DateTime = dateTime;
    }
}
