using SmartWeather.Api.Controllers.User.Dtos;

namespace SmartWeather.Api.Controllers.Authentication.Dtos.Converters
{
    public class UserSigninResponseConverter
    {
        public static UserSigninResponse ConvertUserToUserSigninResponse(SmartWeather.Entities.User.User user, string token)
        {
            return new UserSigninResponse()
            {
                Id = user.Id,
                Role = user.Role,
                Token = token
            };
        }
    }
}
