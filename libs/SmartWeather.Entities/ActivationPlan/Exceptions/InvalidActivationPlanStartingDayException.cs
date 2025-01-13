using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.ActivationPlan.Exceptions;

public class InvalidActivationPlanStartingDayException : Exception
{
    public InvalidActivationPlanStartingDayException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(ActivationPlan),
                                                        "Wrong Starting Day, value must be in [0:6] range"))
    { }
}