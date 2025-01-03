using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Component.Exceptions;

public class InvalidComponentGpioPinException : Exception
{
    public InvalidComponentGpioPinException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Component),
                                                        "Wrong GpioPin, value must be in [0:32] range"))
    { }
}