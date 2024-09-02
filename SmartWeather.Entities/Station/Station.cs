namespace SmartWeather.Entities.Station;
using System;
using SmartWeather.Entities.User;
using SmartWeather.Entities.Component;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string TopicLocation { get; set; }
    public StationType Type { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Component> Components { get; set; } = new List<Component>();


    public Station(string name, float latitude, float longitude, string topicLocation, int userId ,StationType type = StationType.Private) {
        
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name must be filled");
        }
        Name = name;
        
        if (!(-90 <= latitude && latitude <= 90))
        {
            throw new Exception("Latitude value must be contained in [-90;90] range");
        }
        Latitude = latitude;

        if (!(-90 <= longitude && longitude <= 90))
        {
            throw new Exception("Latitude value must be contained in [-90;90] range");
        }
        Longitude = longitude;

        if (String.IsNullOrWhiteSpace(topicLocation))
        {
            throw new Exception("Name must be filled");
        }

        if (userId <= 0)
        {
            throw new Exception("Invalid UserId");
        }
        UserId = userId;
        
        TopicLocation = topicLocation; 
        Type = type;

    }
}
