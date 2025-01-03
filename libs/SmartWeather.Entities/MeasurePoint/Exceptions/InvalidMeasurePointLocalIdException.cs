using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.MeasurePoint.Exceptions;

public class InvalidMeasurePointLocalIdException : Exception
{
    public InvalidMeasurePointLocalIdException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(MeasurePoint),
                                                        "Wrong LocalId, value must be >= 0"))
    { }
}