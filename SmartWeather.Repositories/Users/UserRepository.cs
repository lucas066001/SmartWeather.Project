using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;

namespace SmartWeather.Repositories.Users;

public class UserRepository(Func<SmartWeatherReadOnlyContext> masterContextFactory, Func<SmartWeatherReadOnlyContext> readOnlyContextFactory) : IUserRepository
{
    public User Create(User user)
    {
        using(var masterContext =  masterContextFactory())
        {
            try
            {
                if(masterContext.Users.Where(u => u.Email == user.Email).FirstOrDefault() != null)
                {
                    throw new Exception("Email already taken");
                }

                masterContext.Users.Add(user);
                masterContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to save user in database : " + ex.Message);
            }

            return user;
        }
    }

    public bool Delete(int id)
    {
        using (var masterContext = masterContextFactory())
        {
            bool successfullyDeleted = false;
            try
            {
                User? userToDelete = masterContext.Users.Where(u => u.Id == id).FirstOrDefault();
                if (userToDelete != null)
                {
                    masterContext.Users.Remove(userToDelete);
                    int deleted = masterContext.SaveChanges();
                    successfullyDeleted = deleted == 1;
                }
                else
                {
                    throw new Exception("User not present in database");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to delete user in database : " + ex.Message);
            }

            return successfullyDeleted;
        }
    }

    public IEnumerable<User> GetAll(IEnumerable<int>? ids)
    {
        IEnumerable<User> selectedUsers;
        using (var roContext = readOnlyContextFactory())
        {
            try
            {
                if (ids != null && ids.Any())
                {
                    selectedUsers = roContext.Users.Where(u => ids.Contains(u.Id)).AsEnumerable();
                }
                else
                {
                    selectedUsers = roContext.Users.AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get user list in database : " + ex.Message);
            }
        }

        return selectedUsers;
    }

    public User GetById(int id)
    {
        User? selectedUser;
        using (var roContext = readOnlyContextFactory())
        {
            try
            {
                selectedUser = roContext.Users.Where(u => u.Id == id).FirstOrDefault();
                if (selectedUser == null)
                {
                    throw new Exception("Unknown iuser id");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get user in database : " + ex.Message);
            }
        }
        return selectedUser;
    }

    public User Update(User user)
    {
        using (var masterContext = masterContextFactory())
        {
            try
            {
                masterContext.Users.Update(user);
                masterContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update user in database : " + ex.Message);
            }

            return user;
        }
    }
}
