using System.Text.Json;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;

namespace TerraByte.Infrastructure.External;

public class ViaCepAddressClient(HttpClient httpClient) : IAddressLookupClient
{
    public async Task<AddressLookupResponse?> FetchAddressAsync(string cep)
    {
        var normalizedCep = cep.Replace("-", "").Trim();
        using var response = await httpClient.GetAsync($"ws/{normalizedCep}/json/");

        if (!response.IsSuccessStatusCode)
            return null;

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var root = document.RootElement;

        if (root.TryGetProperty("erro", out var erro) && erro.GetBoolean())
            return null;

        return new AddressLookupResponse
        {
            Cep = root.GetProperty("cep").GetString() ?? normalizedCep,
            Street = root.GetProperty("logradouro").GetString() ?? string.Empty,
            District = root.GetProperty("bairro").GetString() ?? string.Empty,
            City = root.GetProperty("localidade").GetString() ?? string.Empty,
            State = root.GetProperty("uf").GetString() ?? string.Empty
        };
    }
}
