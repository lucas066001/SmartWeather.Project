using SmartWeather.Entities.User;

namespace SmartWeather.Api.Controllers.User.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
    }
}
