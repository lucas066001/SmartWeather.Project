namespace SmartWeather.Services.Authentication;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;

public interface IAuthenticationRepository
{
    /// <summary>
    /// Retreive a User based on given credential.
    /// (Usefull to authenticate User).
    /// </summary>
    /// <param name="hashedPassword">String representing User password.</param>
    /// <param name="email">String representing User email.</param>
    /// <returns>User entity matching credentials.</returns>
    /// <exception cref="EntityFetchingException">Thrown if nothing match. (Credentials incorrect).</exception>
    public User GetUserFromCredential(string hashedPassword, string email);
}
