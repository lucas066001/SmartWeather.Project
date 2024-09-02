namespace SmartWeather.Api.Controllers.User.Dtos
{
    public class UserCreateRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password {  get; set; }
    }
}
