using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;

namespace SmartWeather.Repositories.Users;

public class UserRepository(SmartWeatherReadOnlyContext readOnlyContext) : IUserRepository
{
    public IEnumerable<User> GetAll(IEnumerable<int>? ids)
    {
        IEnumerable<User> selectedUsers;
        try
        {
            if (ids != null && ids.Any())
            {
                selectedUsers = readOnlyContext.Users.Where(u => ids.Contains(u.Id)).AsEnumerable();
            }
            else
            {
                selectedUsers = readOnlyContext.Users.AsEnumerable();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to get user list in database : " + ex.Message);
        }

        return selectedUsers;
    }
}
