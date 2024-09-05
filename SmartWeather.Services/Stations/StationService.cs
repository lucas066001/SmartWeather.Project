using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Services.Repositories;
using SmartWeather.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartWeather.Services.Stations
{
    public class StationService(IRepository<Station> stationBaseRepository, IStationRepository stationRepository)
    {
        public Station AddNewStation(string name, string macAddress, float latitude, float longitude, string topicLocation, int? userId)
        {
            Station stationToCreate = new(name, macAddress, latitude, longitude, topicLocation, userId);
            return stationBaseRepository.Create(stationToCreate);
        }

        public Station AddGenericStation(string macAddress)
        {
            return AddNewStation("Unnamed Station", macAddress, 0.0f, 0.0f, macAddress, null);
        }

        public bool DeleteStation(int idStation)
        {
            return stationBaseRepository.Delete(idStation) != null;
        }

        public Station UpdateStation(int id, string name, string macAddress, float latitude, float longitude, string topicLocation, int userId)
        {
            Station stationToUpdate = new(name, macAddress, latitude, longitude, topicLocation, userId)
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

        //public IEnumerable<User> GetUserList(IEnumerable<int>? idsUser = null)
        //{
        //    return userRepository.GetAll(idsUser);
        //}

        public IEnumerable<Station> GetFromUser(int userId) {
            return stationRepository.GetFromUser(userId);
        }
    }
}
