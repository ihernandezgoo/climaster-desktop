namespace climaster.Models;

public class Location
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsPredefined { get; set; }
    public DateTime? LastUsed { get; set; }

    public string DisplayText => IsPredefined 
        ? $"{Name}" 
        : $"{Name}";

    public override string ToString() => Name;
}

public static class PredefinedLocations
{
    public static List<Location> GetEuskalHerriaLocations()
    {
        return new List<Location>
        {
            new Location { Name = "Donostia", Latitude = 43.31, Longitude = -1.98, IsPredefined = true },
            new Location { Name = "Bilbo", Latitude = 43.26, Longitude = -2.93, IsPredefined = true },
            new Location { Name = "Gasteiz", Latitude = 42.85, Longitude = -2.67, IsPredefined = true },
            new Location { Name = "Iruñea", Latitude = 42.81, Longitude = -1.64, IsPredefined = true },
        };
    }

    public static List<Location> GetSpainLocations()
    {
        return new List<Location>
        {
            new Location { Name = "Madrid", Latitude = 40.42, Longitude = -3.70, IsPredefined = true },
            new Location { Name = "Barcelona", Latitude = 41.39, Longitude = 2.16, IsPredefined = true },
            new Location { Name = "Valencia", Latitude = 39.47, Longitude = -0.38, IsPredefined = true },
            new Location { Name = "Sevilla", Latitude = 37.39, Longitude = -5.98, IsPredefined = true },
            new Location { Name = "Zaragoza", Latitude = 41.65, Longitude = -0.89, IsPredefined = true }
        };
    }
}
