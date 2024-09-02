using SmartWeather.Entities.User;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Repositories.Users
{
    public class UserRepository(SmartWeatherContext context) : IUserRepository
    {
        public User Create(User user)
        {
            try
            {
                if(context.Users.Where(u => u.Email == user.Email).FirstOrDefault() != null)
                {
                    throw new Exception("Email already taken");
                }

                context.Users.Add(user);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to save user in database : " + ex.Message);
            }

            return user;
        }

        public bool Delete(int id)
        {
            bool successfullyDeleted = false;
            try
            {
                User? userToDelete = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    int deleted = context.SaveChanges();
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

        public IEnumerable<User> GetAll(IEnumerable<int>? ids)
        {
            IEnumerable<User> selectedUsers;
            try
            {
                if (ids != null && ids.Any())
                {
                    selectedUsers = context.Users.Where(u => ids.Contains(u.Id)).AsEnumerable();
                }
                else
                {
                    selectedUsers = context.Users.AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get user list in database : " + ex.Message);
            }

            return selectedUsers;
        }

        public User GetById(int id)
        {
            User? selectedUser;
            try
            {
                selectedUser = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (selectedUser == null)
                {
                    throw new Exception("Unknown iuser id");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get user in database : " + ex.Message);
            }

            return selectedUser;
        }

        public User Update(User user)
        {
            try
            {
                context.Users.Update(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update user in database : " + ex.Message);
            }

            return user;
        }
    }
}
