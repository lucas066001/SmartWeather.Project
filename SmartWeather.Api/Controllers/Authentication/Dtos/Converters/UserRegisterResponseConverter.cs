using SmartWeather.Api.Controllers.User.Dtos;

namespace SmartWeather.Api.Controllers.Authentication.Dtos.Converters
{
    public class UserRegisterResponseConverter
    {
        public static UserRegisterResponse ConvertUserToUserRegisterResponse(SmartWeather.Entities.User.User user, string token)
        {
            return new UserRegisterResponse()
            {
                Id = user.Id,
                Role = user.Role,
                Token = token
            };
        }
    }
}
