using System.Globalization;
using System.Text.Json;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;

namespace TerraByte.Infrastructure.External;

public class OpenMeteoGeocodingClient(HttpClient httpClient) : IGeocodingClient
{
    public async Task<GeocodeResponse?> FetchCoordinatesAsync(string location)
    {
        var city = ExtractCity(location);
        var countryFilter = location.Contains("Brasil", StringComparison.OrdinalIgnoreCase)
            ? "&country_code=BR"
            : string.Empty;
        var url = $"v1/search?name={Uri.EscapeDataString(city)}&count=1&language=pt&format=json{countryFilter}";
        using var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        if (!document.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
            return null;

        var first = results[0];
        return new GeocodeResponse
        {
            Name = first.GetProperty("name").GetString() ?? location,
            Latitude = first.GetProperty("latitude").GetDouble(),
            Longitude = first.GetProperty("longitude").GetDouble()
        };
    }

    private static string ExtractCity(string location)
    {
        var parts = location.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => location,
            > 2 => parts[^3],
            _ => parts[0]
        };
    }
}
