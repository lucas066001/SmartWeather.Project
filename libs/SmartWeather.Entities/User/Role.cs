namespace SmartWeather.Entities.User;
public enum Role
{
    Unauthorized,
    Admin,
    User
}

public static class RoleAccess
{
    public static List<Role> GLOBAL_READING_ACCESS = new List<Role>() { Role.Admin };
    public static List<Role> ADMINISTRATORS = new List<Role>() { Role.Admin };

}
