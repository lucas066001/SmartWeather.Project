namespace SmartWeather.Entities.ComponentData;

using SmartWeather.Entities.MeasurePoint;

public class MeasureData
{
    public int MeasurePointId { get; set; }
    public float Value { get; set; }
    public DateTime DateTime { get; set; }

    public MeasureData(int measurePointId, float value, DateTime dateTime)
    {
        MeasurePointId = measurePointId;
        Value = value;
        DateTime = dateTime;
    }
}
