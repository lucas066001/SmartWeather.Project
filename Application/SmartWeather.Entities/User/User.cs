namespace SmartWeather.Entities.User;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using SmartWeather.Entities.Station;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();

    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convertir le mot de passe en bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hacher le mot de passe
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convertir le résultat du hachage en chaîne hexadécimale
            return Convert.ToHexString(hashBytes);
        }
    }

    public User(string username, string email, string passwordHash, Role role = Role.User) {

        if (!EmailRegex.IsMatch(email))
        {
            throw new Exception("Email format is incorrect");
        }

        Email = email;

        if(String.IsNullOrWhiteSpace(passwordHash))
        {
            throw new Exception("Password must be filled");
        }

        PasswordHash = User.HashPassword(passwordHash);

        if (String.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Username must be filled");
        }

        Username = username;
        Role = role;
    }

}
