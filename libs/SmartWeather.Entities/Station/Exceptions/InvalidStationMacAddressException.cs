using SmartWeather.Entities.Common.Exceptions;

namespace SmartWeather.Entities.Station.Exceptions;

public class InvalidStationMacAddressException : Exception
{
    public InvalidStationMacAddressException() : base(string
                                                        .Format(ExceptionsBaseMessages.ENTITY_FORMAT,
                                                        nameof(Station),
                                                        "Wrong Mac address, value must match \"**:**:**:**:**:** \" pattern"))
    { }
}