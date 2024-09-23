namespace SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.MeasurePoint;
using System.Text.RegularExpressions;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null !;
    public ComponentType Type { get; set; }
    public int StationId { get; set; }
    public int GpioPin { get; set; }
    public virtual Station Station { get; set; } = null!;
    public virtual IEnumerable<MeasurePoint> MeasurePoints { get; set; } = null!;

    public static readonly Regex HexColorRegex = new Regex(
        @"#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Component()
    {
    }

    public Component(string name, string color, int type, int stationId, int gpioPin)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name must be filled");
        }
        Name = name;

        if (!HexColorRegex.IsMatch(color))
        {
            throw new Exception("Color format is incorrect");
        }
        Color = color;

        if (!Enum.IsDefined(typeof(ComponentType), type))
        {
            throw new Exception("Component type is incorrect");
        }
        ComponentType foundType = (ComponentType)type;
        Type = foundType;

        if (!(stationId > 0))
        {
            throw new Exception("Invalid StationId");
        }
        StationId = stationId;

        if(!(gpioPin >= 0))
        {
            throw new Exception("Invalid GpioPin");
        }
        GpioPin = gpioPin;
    }
}
