using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Component.Exceptions;

public class InvalidComponentColorException : Exception
{
    public InvalidComponentColorException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Component),
                                                        "Wrong Color, value must be an hexadecimal value"))
    { }
}