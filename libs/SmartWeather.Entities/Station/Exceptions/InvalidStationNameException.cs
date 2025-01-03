using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Station.Exceptions;

public class InvalidStationNameException : Exception
{
    public InvalidStationNameException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Station),
                                                        "Wrong name, value must not be empty"))
    { }
}