using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.MeasurePoint.Exceptions;

public class InvalidMeasurePointUnitException : Exception
{
    public InvalidMeasurePointUnitException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(MeasurePoint),
                                                        "Wrong Unit, value must be registered in application"))
    { }
}