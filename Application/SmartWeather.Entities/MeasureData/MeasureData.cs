namespace SmartWeather.Entities.ComponentData;

using SmartWeather.Entities.MeasurePoint;

public class MeasureData
{
    public int Id { get; set; }
    public int MeasurePointId { get; set; }
    public float Value { get; set; }
    public DateTime DateTime { get; set; }
    public virtual MeasurePoint MeasurePoint { get; set; } = null!;

    public MeasureData(int measurePointId, float value, DateTime dateTime)
    {
        MeasurePointId = measurePointId;
        Value = value;
        DateTime = dateTime;
    }
}
