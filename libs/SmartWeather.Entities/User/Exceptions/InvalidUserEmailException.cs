using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.User.Exceptions;

public class InvalidUserEmailException : Exception
{
    public InvalidUserEmailException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(User),
                                                        "Wrong Email, value must match \"***@**.**\" pattern"))
    { }
}
