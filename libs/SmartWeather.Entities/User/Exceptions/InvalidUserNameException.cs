using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.User.Exceptions;

public class InvalidUserNameException : Exception
{
    public InvalidUserNameException() : base(string
                                                .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                nameof(User),
                                                "Wrong name, value must be not empty"))
    { }
}

