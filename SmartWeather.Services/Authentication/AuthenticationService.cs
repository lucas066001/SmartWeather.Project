using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Entities.User;
using SmartWeather.Services.Options;
using SmartWeather.Services.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Services.Authentication
{
    public class AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IAuthenticationRepository authenticationRepository)
    {
        public string GenerateToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var issuer = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Issuer)];
                var audience = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Audience)];
                var key = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Key)];
                if (string.IsNullOrEmpty(issuer) ||
                    string.IsNullOrEmpty(audience) ||
                    string.IsNullOrEmpty(key))
                {
                    throw new Exception("Unable to retreive structural jwt infos");
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                    Issuer = issuer,
                    Audience = audience,
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception ex)
            {
                throw new Exception("TOKEN ERROR : " + ex.Message);
            }
        }

        public Tuple<User, string> Register(string username, string mail, string password)
        {
            User userToCreate = new(username, mail, password);
            User createdUser = userRepository.Create(userToCreate);
            return new Tuple<User, string>(createdUser, GenerateToken(createdUser));
        }

        public Tuple<User, string> Signin(string email, string password)
        {
            User connectedUser = authenticationRepository.AreCredentialsCorrect(User.HashPassword(password), email);
            return new Tuple<User, string>(connectedUser, GenerateToken(connectedUser));
        }
    }
}
