namespace SmartWeather.Services.Users;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common;

public interface IUserRepository
{
    /// <summary>
    /// Retreives all entities matchings the list of given unique Ids.
    /// </summary>
    /// <param name="ids">List of int representing users unique ids to retreive.</param>
    /// <returns>Result conataining a list of User.</returns>
    public Result<IEnumerable<User>> GetAll(IEnumerable<int> ids);
}
