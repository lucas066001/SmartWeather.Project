namespace SmartWeather.Services.Users;

using SmartWeather.Entities.User;
using System.Collections.Generic;

public class UserService(IUserRepository userRepository)
{
    public User AddNewUser(string username, string mail, string password)
    {
        User userToCreate = new(username, mail, password);
        return userRepository.Create(userToCreate);
    }

    public bool DeleteUser(int idUser)
    {
        return userRepository.Delete(idUser);
    }

    public User UpdateUser(int idUser, string password, string mail, string username, int role = 1)
    {
        User updatedUser = new(username, mail, password, (Role)role)
        {
            Id = idUser
        };
        return userRepository.Update(updatedUser);
    }

    public User GetUserById(int idUser)
    {
        return userRepository.GetById(idUser);
    }

    public IEnumerable<User> GetUserList(IEnumerable<int>? idsUser = null)
    {
        return userRepository.GetAll(idsUser);
    }
}
