namespace SmartWeather.Entities.Station;

using SmartWeather.Entities.User;
using SmartWeather.Entities.Component;
using System.Text.RegularExpressions;
using SmartWeather.Entities.Station.Exceptions;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MacAddress { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public StationType Type { get; set; }
    public int? UserId { get; set; }
    public virtual User? User { get; set; } = null!;
    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    private static readonly Regex MacAddressRegex = new Regex(@"^([0-9A-Fa-f]{2}[:-]){5}[0-9A-Fa-f]{2}$",
                                                              RegexOptions.Compiled | RegexOptions.IgnoreCase);
    /// <summary>
    /// Methods to check if MacAddress format is correct. 
    /// </summary>
    /// <param name="MacAddress">Address to check.</param>
    /// <returns>Boolean whether or not the address match requirements.</returns>
    private bool _checkMacAddress(string MacAddress)
    {
        return MacAddressRegex.IsMatch(MacAddress) || MacAddress.Contains("MOCK_STATION");
    }

    /// <summary>
    /// Create a Station based on given informations.
    /// </summary>
    /// <param name="name">String representing station name.</param>
    /// <param name="macAddress">String representing station mac address.</param>
    /// <param name="latitude">Float representing station latitude coordinate.</param>
    /// <param name="longitude">Float representing station longitude coordinate.</param>
    /// <param name="userId">Nullable Int representing station owner idn could be null if no current owner.</param>
    /// <param name="type">Optional type to specify station type.</param>
    /// <exception cref="InvalidStationNameException">Thrown if station name is empty.</exception>
    /// <exception cref="InvalidStationMacAddressException">Thrown if station mac address doesn't match requirements.</exception>
    /// <exception cref="InvalidStationCoordinatesException">Thrown if station lat/lng isn't in [-90;90] range.</exception>
    /// <exception cref="InvalidStationUserIdException">Thrown if user id is bellow 0.</exception>
    /// <returns>Well formatted Station object.</returns>
    public Station (string name, 
                    string macAddress, 
                    float latitude, 
                    float longitude, 
                    int? userId, 
                    StationType type = StationType.Private) {
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidStationNameException();
        }
        Name = name;

        if (!_checkMacAddress(macAddress))
        {
            throw new InvalidStationMacAddressException();
        }
        MacAddress = macAddress;
        
        if (!(-90 <= latitude && latitude <= 90))
        {
            throw new InvalidStationCoordinatesException();
        }
        Latitude = latitude;

        if (!(-90 <= longitude && longitude <= 90))
        {
            throw new InvalidStationCoordinatesException();
        }
        Longitude = longitude;

        if (userId <= 0)
        {
            throw new InvalidStationUserIdException();
        }
        UserId = userId;
        
        Type = type;
    }
}
