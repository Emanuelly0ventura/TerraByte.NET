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

        var url =
            $"v1/search?name={Uri.EscapeDataString(city)}&count=1&language=pt&format=json{countryFilter}";

        

        using var response = await httpClient.GetAsync(url);

        Console.WriteLine($"Status HTTP: {(int)response.StatusCode} - {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();

    
    

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Requisição falhou.");
            return null;
        }

        using var document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty("results", out var results))
        {
            Console.WriteLine("Campo 'results' não encontrado.");
            return null;
        }

        Console.WriteLine($"Quantidade de resultados: {results.GetArrayLength()}");

        if (results.GetArrayLength() == 0)
        {
            Console.WriteLine("Nenhum resultado encontrado.");
            return null;
        }

        var first = results[0];

        var coordenadas = new RespostaGeocodificacao
        {
            Nome = first.GetProperty("name").GetString() ?? localizacao,
            Latitude = first.GetProperty("latitude").GetDouble(),
            Longitude = first.GetProperty("longitude").GetDouble()
        };
        

        return coordenadas;
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

