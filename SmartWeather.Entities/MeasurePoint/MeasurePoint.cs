namespace SmartWeather.Entities.MeasurePoint;

using SmartWeather.Entities.Component;
using SmartWeather.Entities.ComponentData;

public class MeasurePoint
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public MeasureUnit Unit { get; set; }
    public int ComponentId { get; set; }
    public virtual Component Component { get; set; } = null!;
    public virtual ICollection<MeasureData> MeasureDatas { get; set; } = new List<MeasureData>();

    public MeasurePoint()
    {
    }

    public MeasurePoint(string name, string color, int unit, int componentId) 
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name must be filled");
        }
        Name = name;

        if (!Component.HexColorRegex.IsMatch(color))
        {
            throw new Exception("Color format is incorrect");
        }
        Color = color;

        if (!Enum.IsDefined(typeof(MeasureUnit), unit))
        {
            throw new Exception("Unit type is incorrect");
        }
        MeasureUnit foundUnit = (MeasureUnit)unit;
        Unit = foundUnit;

        if (!(componentId > 0))
        {
            throw new Exception("Invalid ComponentId");
        }
        ComponentId = componentId;
    }

}
