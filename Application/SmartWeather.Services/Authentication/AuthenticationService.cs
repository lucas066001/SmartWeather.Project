using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Entities.User;
using SmartWeather.Services.Options;
using SmartWeather.Services.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace SmartWeather.Services.Authentication;

public class AuthenticationService(IConfiguration configuration, IRepository<User> userBaseRepository, IAuthenticationRepository authenticationRepository)
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
                new Claim(ClaimTypes.Role, ((int)user.Role).ToString())
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

    public int GetUserIdFromToken(string token)
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

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var claimsUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            
            if(int.TryParse(claimsUserId, out int userId))
            {
                return userId;
            }
            else
            {
                throw new SecurityTokenException("Unable to retreive UserId from token");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la validation du token : " + ex.Message);
            return 0;
        }
    }

    public Role GetUserRoleFromToken(string token)
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

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var claimsUserRole = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (int.TryParse(claimsUserRole, out int role))
            {
                return (Role)role;
            }
            else
            {
                throw new SecurityTokenException("Unable to retreive Role from token");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while validating token : " + ex.Message);
            return Role.Unauthorized;
        }
    }



    public Tuple<User, string> Register(string username, string mail, string password)
    {
        User userToCreate = new(username, mail, password);
        User createdUser = userBaseRepository.Create(userToCreate);
        return new Tuple<User, string>(createdUser, GenerateToken(createdUser));
    }

    public Tuple<User, string> Signin(string email, string password)
    {
        User connectedUser = authenticationRepository.AreCredentialsCorrect(User.HashPassword(password), email);
        return new Tuple<User, string>(connectedUser, GenerateToken(connectedUser));
    }
}
