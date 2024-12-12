namespace SmartWeather.Repositories.Users;

using SmartWeather.Entities.User;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;

public class UserRepository(SmartWeatherReadOnlyContext readOnlyContext) : IUserRepository
{
    /// <summary>
    /// Retreives all entities matchings the list of given unique Ids.
    /// </summary>
    /// <param name="ids">List of int representing users unique ids to retreive.</param>
    /// <returns>IEnumerable of User.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<User> GetAll(IEnumerable<int> ids)
    {
        var selectedUsers = readOnlyContext.Users.Where(u => ids.Contains(u.Id)).AsEnumerable();
        return selectedUsers ?? throw new EntityFetchingException();
    }
}
