namespace SmartWeather.Repositories.Users;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;

public class UserRepository(SmartWeatherReadOnlyContext readOnlyContext) : IUserRepository
{
    public IEnumerable<User> GetAll(IEnumerable<int> ids)
    {
        var selectedUsers = readOnlyContext.Users.Where(u => ids.Contains(u.Id)).AsEnumerable();
        return selectedUsers ?? throw new EntityFetchingException();
    }
}
