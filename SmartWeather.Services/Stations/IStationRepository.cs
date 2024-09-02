namespace SmartWeather.Services.Stations;
using SmartWeather.Entities.Station;
using System.Collections.Generic;

public interface IStationRepository
{
    public IEnumerable<Station> GetFromUser(int userId);
}
