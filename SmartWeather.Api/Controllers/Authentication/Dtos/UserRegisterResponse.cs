using SmartWeather.Entities.User;

namespace SmartWeather.Api.Controllers.Authentication.Dtos
{
    public class UserRegisterResponse
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public required string Token { get; set; }
    }
}
