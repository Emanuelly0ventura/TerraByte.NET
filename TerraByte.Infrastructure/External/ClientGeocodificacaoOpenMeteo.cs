using System.Text.Json;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;

namespace TerraByte.Infrastructure.External;

public class ClientGeocodificacaoOpenMeteo(HttpClient httpClient) : IClienteGeocodificacao
{
    public async Task<RespostaGeocodificacao?> BuscarCoordenadasAsync(string localizacao)
    {
        var city = ExtractCity(localizacao);
        var countryFilter = localizacao.Contains("Brasil", StringComparison.OrdinalIgnoreCase)
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
        return new RespostaGeocodificacao
        {
            Nome = first.GetProperty("name").GetString() ?? localizacao,
            Latitude = first.GetProperty("latitude").GetDouble(),
            Longitude = first.GetProperty("longitude").GetDouble()
        };
    }

    private static string ExtractCity(string localizacao)
    {
        var partes = localizacao.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return partes.Length switch
        {
            0 => localizacao,
            > 2 => partes[^3],
            _ => partes[0]
        };
    }
}

