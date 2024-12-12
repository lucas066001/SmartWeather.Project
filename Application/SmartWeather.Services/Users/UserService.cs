namespace SmartWeather.Services.Users;

using SmartWeather.Entities.Station;
using SmartWeather.Entities.User;
using SmartWeather.Services.Repositories;
using System.Collections.Generic;

public class UserService(IRepository<User> userBaseRepository, IUserRepository userRepository)
{
    public User AddNewUser(string username, string mail, string password)
    {
        User userToCreate = new(username, mail, password);
        return userBaseRepository.Create(userToCreate);
    }

    public User DeleteUser(int idUser)
    {
        return userBaseRepository.Delete(idUser);
    }

    public User UpdateUser(int idUser, string password, string mail, string username, int role = 1)
    {
        User updatedUser = new(username, mail, password, (Role)role)
        {
            Id = idUser
        };
        return userBaseRepository.Update(updatedUser);
    }

    public User GetUserById(int idUser)
    {
        return userBaseRepository.GetById(idUser);
    }

    public IEnumerable<User> GetUserList(IEnumerable<int>? idsUser = null)
    {
        return idsUser == null ? userBaseRepository.GetAll() : userRepository.GetAll(idsUser);
    }
}
