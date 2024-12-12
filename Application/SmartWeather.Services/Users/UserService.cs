namespace SmartWeather.Services.Users;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Services.Repositories;
using System.Collections.Generic;
using SmartWeather.Entities.User.Exceptions;

public class UserService(IRepository<User> userBaseRepository, IUserRepository userRepository)
{
    /// <summary>
    /// Create a User in database based on given infos.
    /// </summary>
    /// <param name="username">String representing User username.</param>
    /// <param name="mail">String representing User mail.</param>
    /// <param name="password">String representing User password.</param>
    /// <returns>Created User from database.</returns>
    /// <exception cref="EntityCreationException">Thrown if User informations doesn't meet requirements. (eg:email format)</exception>
    /// <exception cref="EntitySavingException">Thrown if updating doesn't execute properly.</exception>
    public User AddNewUser(string username, string mail, string password)
    {
        try
        {
            User userToCreate = new(username, mail, password);
            return userBaseRepository.Create(userToCreate);
        }
        catch (Exception ex) when (ex is InvalidUserEmailException ||
                                 ex is InvalidUserPasswordException ||
                                 ex is InvalidUserNameException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Delete User based on given unique Id.
    /// </summary>
    /// <param name="idUser">Int representing User unique Id.</param>
    /// <returns>Deleted User from database.</returns>
    /// <exception cref="EntityFetchingException">Thrown if id do not match any existing User.</exception>
    /// <exception cref="EntitySavingException">Thrown if error occurs during User deletion.</exception>
    public bool DeleteUser(int idUser)
    {
        try
        {
            return userBaseRepository.Delete(idUser) != null;
        }
        catch (Exception ex) when (ex is EntityFetchingException ||
                                   ex is EntitySavingException)
        {
            return false;
        }
    }

    /// <summary>
    /// Update an existing User based on given infos.
    /// </summary>
    /// <param name="idUser">Int representing User unique Id.</param>
    /// <param name="password">String repesenting User password.</param>
    /// <param name="mail">String repesenting User email.</param>
    /// <param name="username">String repesenting User username.</param>
    /// <param name="role">Int representing User role.</param>
    /// <returns>Updated User from Database.</returns>
    /// <exception cref="EntityCreationException">Thrown if User informations doesn't meet requirements. (eg:email format)</exception>
    /// <exception cref="EntitySavingException">Thrown if updating doesn't execute properly.</exception>
    public User UpdateUser (int idUser,
                            string password, 
                            string mail, 
                            string username, 
                            int role = (int)Role.User)
    {
        try
        {
            User updatedUser = new(username, mail, password, (Role)role)
            {
                Id = idUser
            };
            return userBaseRepository.Update(updatedUser);

        }
        catch (Exception ex) when (ex is InvalidUserEmailException ||
                                 ex is InvalidUserPasswordException ||
                                 ex is InvalidUserNameException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Retreive User based on given unique Id.
    /// </summary>
    /// <param name="idUser">User unique Id.</param>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    /// <returns>Retreived User from database.</returns>
    public User GetUserById(int idUser)
    {
        return userBaseRepository.GetById(idUser);
    }

    /// <summary>
    /// Retreive Users based on given unique Ids.
    /// If null, no filter applied, all user will be retreived.
    /// </summary>
    /// <param name="idsUser">Optional list of unique User Ids to use as filter.</param>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    /// <returns>Retreived List of User.</returns>
    public IEnumerable<User> GetUserList(IEnumerable<int>? idsUser = null)
    {
        return idsUser == null ? userBaseRepository.GetAll() : userRepository.GetAll(idsUser);
    }
}
