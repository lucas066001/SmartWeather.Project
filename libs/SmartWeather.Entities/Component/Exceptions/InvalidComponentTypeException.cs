using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Component.Exceptions;

public class InvalidComponentTypeException : Exception
{
    public InvalidComponentTypeException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Component),
                                                        "Wrong ComponentType, value must be registered in application"))
    { }
}