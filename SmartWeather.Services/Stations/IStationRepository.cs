using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Services.Stations
{
    public interface IStationRepository
    {
        public IEnumerable<Station> GetFromUser(int userId);
    }
}
