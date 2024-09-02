namespace SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using System.Text.RegularExpressions;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public ComponentUnit Unit { get; set; }
    public ComponentType Type { get; set; }
    public int StationId { get; set; }
    public virtual Station Station { get; set; } = null!;

    private static readonly Regex HexColorRegex = new Regex(
        @"#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Component()
    {
    }

    public Component(string name, string color, int unit, int type, int stationId)
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
        
        if(!Enum.IsDefined(typeof(ComponentUnit), unit))
        {
            throw new Exception("Unit type is incorrect");
        }
        ComponentUnit foundUnit = (ComponentUnit)unit;
        Unit = foundUnit;

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
    }
}
