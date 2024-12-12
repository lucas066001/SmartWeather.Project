namespace SmartWeather.Repositories.Authentication;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Authentication;

public class AuthenticationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IAuthenticationRepository
{
    public User GetUserFromCredential(string hashedPassword, string email)
    {
        var user = readOnlyContext.Users.Where(u => u.Email == email && u.PasswordHash == hashedPassword).FirstOrDefault();
        return user ?? throw new EntityFetchingException();
    }
}
