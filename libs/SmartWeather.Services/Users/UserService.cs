namespace SmartWeather.Services.Users;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Services.Repositories;
using System.Collections.Generic;
using SmartWeather.Entities.User.Exceptions;
using SmartWeather.Entities.Common;

public class UserService(IRepository<User> userBaseRepository, IUserRepository userRepository)
{
    /// <summary>
    /// Create a User in database based on given infos.
    /// </summary>
    /// <param name="username">String representing User username.</param>
    /// <param name="mail">String representing User mail.</param>
    /// <param name="password">String representing User password.</param>
    /// <returns>Result containing created User from database, including auto-generated fields (e.g: id).</returns>
    public Result<User> AddNewUser(string username, string mail, string password)
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
            return Result<User>.Failure(string.Format(
                                            ExceptionsBaseMessages.ENTITY_FORMAT,
                                            nameof(User),
                                            ex.Message));
        }
    }

    /// <summary>
    /// Delete User based on given unique Id.
    /// </summary>
    /// <param name="idUser">Int representing User unique Id.</param>
    /// <returns>Boolean indicating if User has been successfully deleted.</returns>
    public bool DeleteUser(int idUser)
    {
        return userBaseRepository.Delete(idUser).IsSuccess;
    }

    /// <summary>
    /// Update an existing User based on given infos.
    /// </summary>
    /// <param name="idUser">Int representing User unique Id.</param>
    /// <param name="password">String repesenting User password.</param>
    /// <param name="mail">String repesenting User email.</param>
    /// <param name="username">String repesenting User username.</param>
    /// <param name="role">Int representing User role.</param>
    /// <returns>Result containing updated User from Database.</returns>
    public Result<User> UpdateUser (int idUser,
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
            return Result<User>.Failure(string.Format(
                                            ExceptionsBaseMessages.ENTITY_FORMAT,
                                            nameof(User),
                                            ex.Message));
        }
    }

    /// <summary>
    /// Retreive User based on given unique Id.
    /// </summary>
    /// <param name="idUser">User unique Id.</param>
    /// <returns>Result containing retreived User from database.</returns>
    public Result<User> GetUserById(int idUser)
    {
        return userBaseRepository.GetById(idUser);
    }

    /// <summary>
    /// Retreive Users based on given unique Ids.
    /// If null, no filter applied, all user will be retreived.
    /// </summary>
    /// <param name="idsUser">Optional list of unique User Ids to use as filter.</param>
    /// <returns>Result containing retreived List of User.</returns>
    public Result<IEnumerable<User>> GetUserList(IEnumerable<int>? idsUser = null)
    {
        return idsUser == null ? 
                    userBaseRepository.GetAll() : 
                    userRepository.GetAll(idsUser);
    }
}
