using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.User.Exceptions;

public class InvalidUserPasswordException : Exception
{
    public InvalidUserPasswordException() : base(string
                                                .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                nameof(User),
                                                "Wrong Password, value must be not empty"))
    { }
}
