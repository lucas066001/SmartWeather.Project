namespace SmartWeather.Repositories.Authentication;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Authentication;
using SmartWeather.Entities.Common;

public class AuthenticationRepository(SmartWeatherReadOnlyContext readOnlyContext) : IAuthenticationRepository
{
    public Result<User> GetUserFromCredential(string hashedPassword, string email)
    {
        var user = readOnlyContext.Users
                                  .Where(u => u.Email == email && u.PasswordHash == hashedPassword)
                                  .FirstOrDefault();
        return user != null ? 
                Result<User>.Success(user) :
                Result<User>.Failure(string.Format(
                                            ExceptionsBaseMessages.ENTITY_FETCH,
                                            nameof(User)));
    }
}
