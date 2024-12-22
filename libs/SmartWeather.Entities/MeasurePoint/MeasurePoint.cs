namespace SmartWeather.Entities.MeasurePoint;

using SmartWeather.Entities.Component;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.MeasurePoint.Exceptions;

public class MeasurePoint
{
    public int Id { get; set; }
    public int LocalId { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public MeasureUnit Unit { get; set; }
    public int ComponentId { get; set; }
    public virtual Component Component { get; set; } = null!;

    /// <summary>
    /// Empty constructor required to enable EntityFramework creation.
    /// Required because implemented constructor doesn't reference all attributes as parameters.
    /// </summary>
    public MeasurePoint()
    {
    }

    /// <summary>
    /// Create a MeasurePoint based on given informations.
    /// </summary>
    /// <param name="localId">Int representing measure point embded id.</param>
    /// <param name="name">String representing measure point name.</param>
    /// <param name="color">String representing measure point color.</param>
    /// <param name="unit">Int representing measure point unit.</param>
    /// <param name="componentId">Int representing measure point component holder id.</param>
    /// <exception cref="InvalidMeasurePointLocalIdException">Thrown if measure point LocalId is below 1.</exception>
    /// <exception cref="InvalidMeasurePointNameException">Thrown if measure point Name is empty.</exception>
    /// <exception cref="InvalidMeasurePointColorException">Thrown if measure point Color isn't hex format.</exception>
    /// <exception cref="InvalidMeasurePointUnitException">Thrown if measure point Unit isn't known by the platform. </exception>
    /// <exception cref="InvalidMeasurePointComponentIdException">Thrown if measure point ComponentId is below 1.</exception>
    /// <returns>A well formatted MeasurePoint object.</returns>
    public MeasurePoint (int localId, 
                         string name, 
                         string color, 
                         int unit, 
                         int componentId) 
    {
        if (localId <= 0)
        {
            throw new InvalidMeasurePointLocalIdException();
        }
        LocalId = localId;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidMeasurePointNameException();
        }
        Name = name;

        if (!Component.HexColorRegex.IsMatch(color))
        {
            throw new InvalidMeasurePointColorException();
        }
        Color = color;

        if (!Enum.IsDefined(typeof(MeasureUnit), unit))
        {
            throw new InvalidMeasurePointUnitException();
        }
        MeasureUnit foundUnit = (MeasureUnit)unit;
        Unit = foundUnit;

        if (componentId <= 0)
        {
            throw new InvalidMeasurePointComponentIdException();
        }
        ComponentId = componentId;
    }

}
