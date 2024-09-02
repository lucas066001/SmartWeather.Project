using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Repositories.Authentication
{
    public class AuthenticationRepository(SmartWeatherContext context) : IAuthenticationRepository
    {
        public User AreCredentialsCorrect(string hashedPassword, string email)
        {
            User connectedUser;
            try
            {
                var tmpUser = context.Users.Where(u => u.Email == email && u.PasswordHash == hashedPassword).FirstOrDefault();
                
                if(tmpUser != null)
                {
                    connectedUser = tmpUser;
                }
                else
                {
                    throw new Exception("Credential incorrect, unable to authenticate user from given infos");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Authentication error : " + ex.Message);
            }

            return connectedUser;
        }
    }
}
