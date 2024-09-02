using System.Linq;

namespace SmartWeather.Api.Controllers.User.Dtos.Converters
{
    public class UserResponseConverter
    {
        public static UserResponse ConvertUserToUserResponse(SmartWeather.Entities.User.User user)
        {
            return new UserResponse() { 
                Email = user.Email,
                Name = user.Username, 
                Id = user.Id, 
                Role = user.Role
            };
        }

        public static UserListResponse ConvertUserListToUserListResponse(IEnumerable<SmartWeather.Entities.User.User> users)
        {
            UserListResponse response = new UserListResponse() { UserList = new List<UserResponse>() };

            foreach (var user in users)
            {
                var test = ConvertUserToUserResponse(user);
                response.UserList.Add(test);
            }
            return response;
        }
    }
}
