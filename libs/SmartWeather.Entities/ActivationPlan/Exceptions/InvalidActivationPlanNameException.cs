using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.ActivationPlan.Exceptions;

public class InvalidActivationPlanNameException : Exception
{
    public InvalidActivationPlanNameException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(ActivationPlan),
                                                        "Wrong Name, value must be not null"))
    { }
}