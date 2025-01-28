using System.Text.Json.Serialization;

namespace SmartWeather.Entities.ComponentData;

public class MeasureData
{
    [JsonPropertyName("measurePointId")]
    public int MeasurePointId { get; set; }

    [JsonPropertyName("value")]
    public float Value { get; set; }
    
    [JsonPropertyName("dateTime")]
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
