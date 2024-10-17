using SmartWeather.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Services.Authentication
{
    public interface IAuthenticationRepository
    {
        public User AreCredentialsCorrect(string hashedPassword, string email);
    }
}
