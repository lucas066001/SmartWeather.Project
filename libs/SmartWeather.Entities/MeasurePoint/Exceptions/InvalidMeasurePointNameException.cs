using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.MeasurePoint.Exceptions;

public class InvalidMeasurePointNameException : Exception
{
    public InvalidMeasurePointNameException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(MeasurePoint),
                                                        "Wrong name, value must be not empty"))
    { }
}