using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Station.Exceptions;

public class InvalidStationUserIdException : Exception
{
    public InvalidStationUserIdException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Station),
                                                        "Wrong UserId, value must correspond to an existing user"))
    { }
}