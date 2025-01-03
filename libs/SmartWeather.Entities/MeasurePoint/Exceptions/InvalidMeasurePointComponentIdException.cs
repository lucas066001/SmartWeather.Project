using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.MeasurePoint.Exceptions;

public class InvalidMeasurePointComponentIdException : Exception
{
    public InvalidMeasurePointComponentIdException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(MeasurePoint),
                                                        "Wrong ComponentId, value must correspond to an existing component"))
    { }
}