namespace SmartWeather.Entities.ComponentData;

using SmartWeather.Entities.Component;

public class ComponentData
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public int Value { get; set; }
    public DateTime DateTime { get; set; }
    public virtual Component Component { get; set; } = null!;

    public ComponentData(int componentId, int value, DateTime dateTime)
    {
        ComponentId = componentId;
        Value = value;
        DateTime = dateTime;
    }
}
