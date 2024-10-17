using SmartWeather.Entities.User;

namespace SmartWeather.Api.Controllers.User.Dtos
{
    public class UserCreateResponse
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public required string Token { get; set; }
    }
}
