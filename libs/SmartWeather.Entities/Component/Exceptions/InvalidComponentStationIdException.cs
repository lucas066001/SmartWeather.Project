using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Component.Exceptions;

public class InvalidComponentStationIdException : Exception
{
    public InvalidComponentStationIdException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Component),
                                                        "Wrong StationId, value must correspond to an existing station"))
    { }
}