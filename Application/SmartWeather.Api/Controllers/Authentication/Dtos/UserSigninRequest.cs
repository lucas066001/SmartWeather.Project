namespace SmartWeather.Api.Controllers.Authentication.Dtos
{
    public class UserSigninRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
