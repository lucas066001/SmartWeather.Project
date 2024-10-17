namespace SmartWeather.Api.Controllers.User.Dtos
{
    public class UserUpdateRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password {  get; set; }
        public int Id {  get; set; }
    }
}
