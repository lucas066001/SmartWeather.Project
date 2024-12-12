namespace SmartWeather.Services.Stations;
using SmartWeather.Entities.Station;
using System.Collections.Generic;

public interface IStationRepository
{
    public IEnumerable<Station> GetFromUser(int userId);
    public Station GetByMacAddress(string macAddress);
    public bool IsOwnerOfStation(int userId, int idStation);
    public IEnumerable<Station> GetAll(bool includeComponents, bool includeMeasurePoint);


}
