namespace SmartWeather.Services.Users;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;

public interface IUserRepository
{
    /// <summary>
    /// Retreives all entities matchings the list of given unique Ids.
    /// </summary>
    /// <param name="ids">List of int representing users unique ids to retreive.</param>
    /// <returns>IEnumerable of User.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<User> GetAll(IEnumerable<int> ids);
}
