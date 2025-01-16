namespace SmartWeather.Entities.ActivationPlan;

using SmartWeather.Entities.ActivationPlan.Exceptions;
using SmartWeather.Entities.Component;
using System;

public class ActivationPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
    public TimeSpan ActivationTime { get; set; }
    public TimeSpan Duration { get; set; }
    public int PeriodInDay { get; set; }
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
    /// <param name="startingDate">DateTime indicating when the plan will start.</param>
    /// <param name="endingDate">DaTime indicating when the plan will end.</param>
    /// <param name="periodInDay">TimeSpan representing the time elapsed between two activations.</param>
    /// <param name="activationTime">TimeSpan representing the time an activativation will start in the day.</param>
    /// <param name="duration">TimeSpan representing the time an activativation will last.</param>
    /// <param name="componentId">Int representing component unique Id to activate.</param>
    /// <exception cref="InvalidActivationPlanNameException">Thrown if activation plan name is null or empty.</exception>
    /// <exception cref="InvalidActivationPlanComponentIdException">Thrown if activation plan componentId is below 1.</exception>
    /// <returns>A well formatted ActivationPlan object.</returns>
    public ActivationPlan(string name, 
                          DateTime startingDate, 
                          DateTime endingDate,
                          int periodInDay,
                          TimeSpan activationTime, 
                          TimeSpan duration,
                          int componentId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidActivationPlanNameException();
        }
        Name = name;

        StartingDate = startingDate;
        EndingDate = endingDate;
        ActivationTime = activationTime;

        if (!(componentId > 0))
        {
            throw new InvalidActivationPlanComponentIdException();
        }
        ComponentId = componentId;

        PeriodInDay = periodInDay;
        Duration = duration;
    }
}
