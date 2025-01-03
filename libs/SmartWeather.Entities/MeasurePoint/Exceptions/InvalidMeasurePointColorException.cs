using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.MeasurePoint.Exceptions;

public class InvalidMeasurePointColorException : Exception
{
    public InvalidMeasurePointColorException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(MeasurePoint),
                                                        "Wrong Color, value must be hexadecimal format"))
    { }
}