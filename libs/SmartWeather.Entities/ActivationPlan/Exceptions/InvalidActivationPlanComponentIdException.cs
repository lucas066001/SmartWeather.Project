using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.ActivationPlan.Exceptions;

public class InvalidActivationPlanComponentIdException : Exception
{
    public InvalidActivationPlanComponentIdException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(ActivationPlan),
                                                        "Wrong Component Id, value must be > 0"))
    { }
}