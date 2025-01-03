namespace SmartWeather.Services.Authentication;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.Common;

public interface IAuthenticationRepository
{
    /// <summary>
    /// Retreive a User based on given credential.
    /// (Usefull to authenticate User).
    /// </summary>
    /// <param name="hashedPassword">String representing User password.</param>
    /// <param name="email">String representing User email.</param>
    /// <returns>Result entity containing User that matched credentials.</returns>
    public Result<User> GetUserFromCredential(string hashedPassword, string email);
}
