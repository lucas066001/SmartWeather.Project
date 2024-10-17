using SmartWeather.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Services.Users
{
    public interface IUserRepository
    {
        public User Create(User user);
        public bool Delete(int id);
        public User Update(User user);
        public IEnumerable<User> GetAll(IEnumerable<int>? ids);
        public User GetById(int id);
    }
}
