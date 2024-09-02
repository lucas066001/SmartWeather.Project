namespace SmartWeather.Api.Controllers.Authentication.Dtos
{
    public class UserRegisterRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
