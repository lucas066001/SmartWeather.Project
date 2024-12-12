using SmartWeather.Entities.User;
namespace SmartWeather.Services.Users;

public interface IUserRepository
{
    public IEnumerable<User> GetAll(IEnumerable<int>? ids);
}
