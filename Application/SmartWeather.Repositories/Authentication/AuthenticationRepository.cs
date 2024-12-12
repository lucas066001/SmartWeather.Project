namespace SmartWeather.Repositories.Authentication;

using SmartWeather.Entities.User;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Authentication;

public class AuthenticationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IAuthenticationRepository
{
    /// <summary>
    /// Retreive a User based on given credential.
    /// (Usefull to authenticate User).
    /// </summary>
    /// <param name="hashedPassword">String representing User password.</param>
    /// <param name="email">String representing User email.</param>
    /// <returns>User entity matching credentials.</returns>
    /// <exception cref="EntityFetchingException">Thrown if nothing match. (Credentials incorrect).</exception>
    public User GetUserFromCredential(string hashedPassword, string email)
    {
        var user = readOnlyContext.Users.Where(u => u.Email == email && u.PasswordHash == hashedPassword).FirstOrDefault();
        return user ?? throw new EntityFetchingException();
    }
}
