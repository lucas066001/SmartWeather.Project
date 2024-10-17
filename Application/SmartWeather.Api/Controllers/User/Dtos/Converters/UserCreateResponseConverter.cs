namespace SmartWeather.Api.Controllers.User.Dtos.Converters
{
    public class UserCreateResponseConverter
    {
        public static UserCreateResponse ConvertUserToUserCreateResponse(SmartWeather.Entities.User.User user, string token)
        {
            return new UserCreateResponse()
            {
                Id = user.Id,
                Role = user.Role,
                Token = token
            };
        }
    }
}
