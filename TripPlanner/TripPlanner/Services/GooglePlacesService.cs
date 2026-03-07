using System.Text.Json;

namespace TripPlanner.Services;

public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public GooglePlacesService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<GooglePlaceResult>> SearchGooglePlaces(string query)
    {
        var apikey = _config["GoogleMaps:GoogleApiKey"];

        var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={{query}}&key={{apiKey}}";
        
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new List<GooglePlaceResult>();
        
        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var result = JsonSerializer.Deserialize<GooglePlaceResponse>(json, options);

        return result?.Results ?? new List<GooglePlaceResult>();
    }
}

public class GooglePlaceResponse
{
    public List<GooglePlaceResult> Results { get; set; } = new();
}

public class GooglePlaceResult
{
    public string Name { get; set; } = "";
    public string Formatted_Address { get; set; } = "";
    public string Place_Id { get; set; } = "";
    public Geometry Geometry { get; set; } = new();
}

public class Geometry
{
    public Location Location { get; set; } = new();
}

public class Location
{
    public double Lat { get; set; }
    public double Lng { get; set; }
} 