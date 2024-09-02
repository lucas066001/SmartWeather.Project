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
        public Station AddNewStation(string name, float latitude, float longitude, string topicLocation, int userId)
        {
            Station stationToCreate = new(name, latitude, longitude, topicLocation, userId);
            return stationBaseRepository.Create(stationToCreate);
        }

        public bool DeleteStation(int idStation)
        {
            return stationBaseRepository.Delete(idStation) != null;
        }

        public Station UpdateStation(int id, string name, float latitude, float longitude, string topicLocation, int userId)
        {
            Station stationToUpdate = new(name, latitude, longitude, topicLocation, userId)
            {
                Id = id
            };
            return stationBaseRepository.Update(stationToUpdate);
        }

        //public User GetUserById(int idUser)
        //{
        //    return userRepository.GetById(idUser);
        //}

        //public IEnumerable<User> GetUserList(IEnumerable<int>? idsUser = null)
        //{
        //    return userRepository.GetAll(idsUser);
        //}

        public IEnumerable<Station> GetFromUser(int userId) {
            return stationRepository.GetFromUser(userId);
        }
    }
}
