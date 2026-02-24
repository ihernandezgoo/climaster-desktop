using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace climaster.Services;

public class LocationService
{
    private readonly HttpClient _httpClient;

    public LocationService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CliMaster/1.0");
    }

    // Oraingo kokapena lortu IP geokokapen APIa erabiliz
    public async Task<(double Latitude, double Longitude, string LocationName)?> GetCurrentLocationAsync()
    {
        try
        {
            // ipapi.co erabili IPan oinarritutako kokapena lortzeko
            var response = await _httpClient.GetStringAsync("https://ipapi.co/json/");
            var locationData = JsonSerializer.Deserialize<IpApiResponse>(response);

            if (locationData != null && locationData.Latitude != 0 && locationData.Longitude != 0)
            {
                var cityName = !string.IsNullOrEmpty(locationData.City) 
                    ? locationData.City 
                    : locationData.Region ?? "Oraingo Kokapena";

                return (locationData.Latitude, locationData.Longitude, cityName);
            }

            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Errorea kokapena lortzean: {ex.Message}");
            
            // Fallback: beste zerbitzu batekin saiatu
            return await GetLocationFromIpifyAsync();
        }
    }

    private async Task<(double Latitude, double Longitude, string LocationName)?> GetLocationFromIpifyAsync()
    {
        try
        {
            // IP publikoa lortu
            var ipResponse = await _httpClient.GetStringAsync("https://api.ipify.org");
            
            // Geokokapen datuak ip-api.com-etik lortu
            var geoResponse = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ipResponse}");
            var geoData = JsonSerializer.Deserialize<IpApiComResponse>(geoResponse);

            if (geoData != null && geoData.Status == "success")
            {
                return (geoData.Lat, geoData.Lon, geoData.City ?? "Oraingo Kokapena");
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    // Egiaztatu kokapen-zerbitzua erabilgarri dagoen
    public async Task<bool> IsLocationAvailableAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://ipapi.co/json/", HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // Deserializaziorako ereduak
    private class IpApiResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("city")]
        public string? City { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("region")]
        public string? Region { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("country_name")]
        public string? CountryName { get; set; }
    }

    private class IpApiComResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("lat")]
        public double Lat { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("lon")]
        public double Lon { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("city")]
        public string? City { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("regionName")]
        public string? RegionName { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("country")]
        public string? Country { get; set; }
    }
}
