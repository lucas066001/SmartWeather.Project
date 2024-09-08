namespace SmartWeather.Services.Stations;

using SmartWeather.Entities.Station;
using SmartWeather.Services.Repositories;

public class StationService(IRepository<Station> stationBaseRepository, IStationRepository stationRepository)
{
    public Station AddNewStation(string name, string macAddress, float latitude, float longitude, int? userId)
    {
        Station stationToCreate = new(name, macAddress, latitude, longitude, userId);
        return stationBaseRepository.Create(stationToCreate);
    }

    public Station AddGenericStation(string macAddress)
    {
        return AddNewStation("Unnamed Station", macAddress, 0.0f, 0.0f, null);
    }

    public bool DeleteStation(int idStation)
    {
        return stationBaseRepository.Delete(idStation) != null;
    }

    public Station UpdateStation(int id, string name, string macAddress, float latitude, float longitude, int userId)
    {
        Station stationToUpdate = new(name, macAddress, latitude, longitude, userId)
        {
            Id = id
        };
        return stationBaseRepository.Update(stationToUpdate);
    }

    public Station GetStationById(int idStation)
    {
        return stationBaseRepository.GetById(idStation);
    }

    public Station? GetStationByMacAddress(string macAddress)
    {
        return stationRepository.GetByMacAddress(macAddress);
    }

    public bool IsStationRegistered(string macAddress)
    {
        return stationRepository.GetByMacAddress(macAddress) == null;
    }

    public IEnumerable<Station> GetAll()
    {
        return stationBaseRepository.GetAll();
    }

    public IEnumerable<Station> GetFromUser(int userId) {
        return stationRepository.GetFromUser(userId);
    }
}
