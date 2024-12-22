namespace SmartWeather.Entities.User;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.User.Exceptions;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public virtual ICollection<Station> Stations { get; set; } = [];

    private static readonly Regex EmailRegex = new (@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                                                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
    /// <summary>
    /// Take a string and hash it to create an irreversible version of it.
    /// </summary>
    /// <param name="password">Original string.</param>
    /// <returns>Hashed version of string.</returns>
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToHexString(hashBytes);
        }
    }

    /// <summary>
    /// Create a User based on given informations.
    /// </summary>
    /// <param name="username">String representing User name.</param>
    /// <param name="email">String representing User email.</param>
    /// <param name="passwordHash">String representing User password.</param>
    /// <param name="role">Optional role to adjust user access.</param>
    /// <exception cref="InvalidUserEmailException">Thrown if email doesn't match requirements.</exception>
    /// <exception cref="InvalidUserPasswordException">Thrown if password is empty.</exception>
    /// <exception cref="InvalidUserNameException">Thrown if username is empty.</exception>
    /// <returns>Well formatted User object.</returns>
    public User (string username, 
                 string email, 
                 string passwordHash, 
                 Role role = Role.User) {

        if (!EmailRegex.IsMatch(email))
        {
            throw new InvalidUserEmailException();
        }
        Email = email;

        if(string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new InvalidUserPasswordException();
        }
        PasswordHash = HashPassword(passwordHash);

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidUserNameException();
        }
        Username = username;

        Role = role;
    }

}
