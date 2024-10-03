using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Authentication;

namespace SmartWeather.Repositories.Authentication;

public class AuthenticationRepository(Func<SmartWeatherReadOnlyContext> contextFactory) : IAuthenticationRepository
{
    public User AreCredentialsCorrect(string hashedPassword, string email)
    {
        User connectedUser;
        using (var roContext = contextFactory())
        {
            try
            {
                var tmpUser = roContext.Users.Where(u => u.Email == email && u.PasswordHash == hashedPassword).FirstOrDefault();
            
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
        }

        return connectedUser;
    }
}
