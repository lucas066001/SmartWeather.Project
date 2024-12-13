using SmartWeather.Entities.User;

namespace SmartWeather.Api.Controllers.User.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Role Role { get; set; }
    }
}
