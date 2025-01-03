using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Station.Exceptions;

public class InvalidStationCoordinatesException : Exception
{
    public InvalidStationCoordinatesException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Station),
                                                        "Wrong coordinates, value must be in [-90:90] range")){ }
}


