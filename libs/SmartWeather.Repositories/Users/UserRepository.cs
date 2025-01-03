namespace SmartWeather.Repositories.Users;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;
using SmartWeather.Entities.Common;

public class UserRepository(SmartWeatherReadOnlyContext readOnlyContext) : IUserRepository
{
    public Result<IEnumerable<User>> GetAll(IEnumerable<int> ids)
    {
        var selectedUsers = readOnlyContext.Users
                                        .Where(u => ids.Contains(u.Id))
                                        .AsEnumerable();
        return selectedUsers != null ?
                    Result<IEnumerable<User>>.Success(selectedUsers) :
                    Result<IEnumerable<User>>.Failure(string.Format(
                                                                ExceptionsBaseMessages.ENTITY_FETCH,
                                                                nameof(User)));
    }
}
