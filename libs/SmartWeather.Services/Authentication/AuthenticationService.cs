namespace SmartWeather.Services.Authentication;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.User;
using SmartWeather.Entities.User.Exceptions;
using SmartWeather.Services.Options;
using SmartWeather.Services.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

public class AuthenticationService(IConfiguration configuration, IRepository<User> userBaseRepository, IAuthenticationRepository authenticationRepository)
{
    /// <summary>
    /// Generate validation parameters for app token.
    /// </summary>
    /// <returns>TokenValidationParameters object from app config.</returns>
    /// <exception cref="FieldAccessException">Thrown if unable to retreive structural infos for token generation.</exception>
    private TokenValidationParameters _getTokenValidationParameters() {
        var issuer = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Issuer)];
        var audience = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Audience)];
        var key = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Key)];
        
        if (string.IsNullOrEmpty(issuer) ||
            string.IsNullOrEmpty(audience) ||
            string.IsNullOrEmpty(key))
        {
            throw new FieldAccessException("Unable to retreive structural jwt infos");
        }

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    }

    /// <summary>
    /// Generate a token based on app config and given User object.
    /// </summary>
    /// <param name="user">User to create access to.</param>
    /// <returns>String representing token</returns>
    /// <exception cref="FieldAccessException">Thrown if unable to access structural JWT configuration infos.</exception>
    /// <exception cref="EntityCreationException">Thrown if unable to create token.</exception>
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var issuer = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Issuer)];
        var audience = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Audience)];
        var key = configuration.GetSection(nameof(Jwt))[nameof(Jwt.Key)];
        if (string.IsNullOrEmpty(issuer) ||
            string.IsNullOrEmpty(audience) ||
            string.IsNullOrEmpty(key))
        {
            throw new FieldAccessException("Unable to retreive structural jwt infos");
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

        try
        {
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
        catch (Exception ex) when (ex is ArgumentNullException ||
                                   ex is ArgumentException ||
                                   ex is SecurityTokenEncryptionFailedException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Retreive User unique Id from given token.
    /// </summary>
    /// <param name="token">String representing token.</param>
    /// <returns>Retreived User unique Id from token.</returns>
    /// <exception cref="SecurityTokenException">Thrown if unable to read token.</exception>
    public int GetUserIdFromToken(string token)
    {
        var validationParameters = _getTokenValidationParameters();

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
        catch (Exception)
        {
            throw new SecurityTokenException("Unable to validate token");
        }

        var jwtToken = validatedToken as JwtSecurityToken ?? throw new SecurityTokenException("Invalid token");

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

    /// <summary>
    /// Retreive User Role from given token.
    /// </summary>
    /// <param name="token">String representing token.</param>
    /// <returns>Retreived User unique Id from token.</returns>
    /// <exception cref="SecurityTokenException">Thrown if unable to read token.</exception>
    public Role GetUserRoleFromToken(string token)
    {
        var validationParameters = _getTokenValidationParameters();
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
        catch (Exception)
        {
            throw new SecurityTokenException("Unable to validate token");
        }

        var jwtToken = validatedToken as JwtSecurityToken ?? throw new SecurityTokenException("Invalid token");

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

    /// <summary>
    /// Create a User based on given infos and generate a token from it.
    /// </summary>
    /// <param name="username">String representing User username.</param>
    /// <param name="mail">String representing User mail.</param>
    /// <param name="password">String representing User password.</param>
    /// <returns>A Tuple formed with User entity and it's corresponding token.</returns>
    /// <exception cref="EntityCreationException">Thrown if Unable to create User or Token.</exception>
    /// <exception cref="EntitySavingException">Thrown if error occurs during saving User in database.</exception>
    public Tuple<User, string> Register(string username, string mail, string password)
    {
        try
        {
            User userToCreate = new(username, mail, password);
            User createdUser = userBaseRepository.Create(userToCreate);
            return new Tuple<User, string>(createdUser, GenerateToken(createdUser));
        }
        catch (Exception ex) when (ex is InvalidUserEmailException ||
                                   ex is InvalidUserPasswordException ||
                                   ex is InvalidUserNameException || 
                                   ex is FieldAccessException)
        {
            throw new EntityCreationException();
        }
    }

    /// <summary>
    /// Retreive a User from given crendentials and generate a token.
    /// </summary>
    /// <param name="email">String representing User email to check.</param>
    /// <param name="password">String representing User password to check.</param>
    /// <returns>A Tuple formed with User entity and it's corresponding token.</returns>
    /// <exception cref="EntityFetchingException">Thrown if credentials are incorrects.</exception>
    /// <exception cref="EntityCreationException">Thrown if errors occurs during token creation.</exception>
    public Tuple<User, string> Signin(string email, string password)
    {
        try
        {
            User connectedUser = authenticationRepository.GetUserFromCredential(User.HashPassword(password), email);
            return new Tuple<User, string>(connectedUser, GenerateToken(connectedUser));
        }
        catch(FieldAccessException)
        {
            throw new EntityCreationException();
        }
    }
}
