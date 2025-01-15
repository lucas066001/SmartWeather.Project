namespace SmartWeather.Entities.ActivationPlan;

using SmartWeather.Entities.ActivationPlan.Exceptions;
using SmartWeather.Entities.Component;

public class ActivationPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DayOfWeek StartingDay { get; set; }
    public TimeSpan Period { get; set; }
    public TimeSpan Duration { get; set; }
    public int ComponentId { get; set; }
    public virtual Component Component { get; set; } = null!;

    /// <summary>
    /// Empty constructor required to enable EntityFramework creation.
    /// Required because implemented constructor doesn't reference all attributes as parameters.
    /// </summary>
    public ActivationPlan()
    {
    }

    /// <summary>
    /// Create an ActivationPlan based on given informations.
    /// </summary>
    /// <param name="name">String representing activation human readable name.</param>
    /// <param name="startingDay">Day indicating when the plan will start.</param>
    /// <param name="period">TimeSpan representing the time elapsed between two activations.</param>
    /// <param name="duration">TimeSpan representing the time an activativation will last.</param>
    /// <param name="componentId">Int representing component unique Id to activate.</param>
    /// <exception cref="InvalidActivationPlanNameException">Thrown if activation plan name is null or empty.</exception>
    /// <exception cref="InvalidActivationPlanStartingDayException">Thrown if activation plan starting day isn't part of predefined enum DayOfWeek.</exception>
    /// <exception cref="InvalidActivationPlanComponentIdException">Thrown if activation plan componentId is below 1.</exception>
    /// <returns>A well formatted ActivationPlan object.</returns>
    public ActivationPlan(string name, 
                          int startingDay, 
                          TimeSpan period, 
                          TimeSpan duration, 
                          int componentId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidActivationPlanNameException();
        }
        Name = name;

        if (!Enum.IsDefined(typeof(DayOfWeek), startingDay))
        {
            throw new InvalidActivationPlanStartingDayException();
        }
        DayOfWeek foundStartingDay = (DayOfWeek)startingDay;
        StartingDay = foundStartingDay;

        if (!(componentId > 0))
        {
            throw new InvalidActivationPlanComponentIdException();
        }
        ComponentId = componentId;

        Period = period;
        Duration = duration;
    }
}
