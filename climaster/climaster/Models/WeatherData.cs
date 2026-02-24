using System.Text.Json.Serialization;

namespace climaster.Models;

public class WeatherData
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [JsonPropertyName("current")]
    public CurrentWeather? Current { get; set; }

    [JsonPropertyName("hourly")]
    public List<HourlyWeather>? Hourly { get; set; }

    [JsonPropertyName("daily")]
    public List<DailyWeather>? Daily { get; set; }
}

public class CurrentWeather
{
    [JsonPropertyName("dt")]
    public long DateTime { get; set; }

    [JsonPropertyName("temp")]
    public double Temperature { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("wind_speed")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescription>? Weather { get; set; }
}

public class HourlyWeather
{
    [JsonPropertyName("dt")]
    public long DateTime { get; set; }

    [JsonPropertyName("temp")]
    public double Temperature { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescription>? Weather { get; set; }
}

public class DailyWeather
{
    [JsonPropertyName("dt")]
    public long DateTime { get; set; }

    [JsonPropertyName("temp")]
    public DailyTemperature? Temp { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescription>? Weather { get; set; }
}

public class DailyTemperature
{
    [JsonPropertyName("day")]
    public double Day { get; set; }

    [JsonPropertyName("min")]
    public double Min { get; set; }

    [JsonPropertyName("max")]
    public double Max { get; set; }
}

public class WeatherDescription
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("main")]
    public string Main { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}
