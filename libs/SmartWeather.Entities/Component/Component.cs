namespace SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.MeasurePoint;
using System.Text.RegularExpressions;
using SmartWeather.Entities.Component.Exceptions;
using SmartWeather.Entities.ActivationPlan;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public ComponentType Type { get; set; }
    public int StationId { get; set; }
    public int GpioPin { get; set; }
    public virtual Station Station { get; set; } = null!;
    public virtual ICollection<MeasurePoint> MeasurePoints { get; set; } = [];
    public virtual ICollection<ActivationPlan> ActivationPlans { get; set; } = [];

    public static readonly Regex HexColorRegex = new Regex(@"#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
                                                           RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Empty constructor required to enable EntityFramework creation.
    /// Required because implemented constructor doesn't reference all attributes as parameters.
    /// </summary>
    public Component()
    {
    }

    /// <summary>
    /// Create a Component based on given informations.
    /// </summary>
    /// <param name="name">String representing component name.</param>
    /// <param name="color">String representing component color.</param>
    /// <param name="type">Int representing component type.</param>
    /// <param name="stationId">Int representing station id that hold component.</param>
    /// <param name="gpioPin">Int representing embded component pin.</param>
    /// <exception cref="InvalidComponentNameException">Thrown if component Name is empty.</exception>
    /// <exception cref="InvalidComponentColorException">Thrown if component Color isn't hex format.</exception>
    /// <exception cref="InvalidComponentTypeException">Thrown if component Type isn't supported by platform.</exception>
    /// <exception cref="InvalidComponentStationIdException">Thrown if component StationId is below 1.</exception>
    /// <exception cref="InvalidComponentGpioPinException">Thrown if component GpioPin isn't in [0;32] range.</exception>
    /// <returns>A well formatted Component object.</returns>
    public Component (string name, 
                      string color, 
                      int type, 
                      int stationId, 
                      int gpioPin)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidComponentNameException();
        }
        Name = name;

        if (!HexColorRegex.IsMatch(color))
        {
            throw new InvalidComponentColorException();
        }
        Color = color;

        if (!Enum.IsDefined(typeof(ComponentType), type))
        {
            throw new InvalidComponentTypeException();
        }
        ComponentType foundType = (ComponentType)type;
        Type = foundType;

        if (stationId <= 0)
        {
            throw new InvalidComponentStationIdException();
        }
        StationId = stationId;

        if(!(0 <= gpioPin && gpioPin <= 39))
        {
            throw new InvalidComponentGpioPinException();
        }
        GpioPin = gpioPin;
    }
}
