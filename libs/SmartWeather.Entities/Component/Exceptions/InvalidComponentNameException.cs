using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Component.Exceptions;

public class InvalidComponentNameException : Exception
{
    public InvalidComponentNameException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Component),
                                                        "Wrong name, value must be not empty"))
    { }
}