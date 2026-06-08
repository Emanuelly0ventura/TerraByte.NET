using System.Text.Json;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;

namespace TerraByte.Infrastructure.External;

public class ClientEnderecoViaCep(HttpClient httpClient) : IExternalApiClient
{
    private static string GetStringOrEmpty(JsonElement root, string propertyName)
    {
        return root.TryGetProperty(propertyName, out var property)
            ? property.GetString() ?? string.Empty
            : string.Empty;
    }
    public async Task<RespostaConsultaEndereco?> BuscarEnderecoAsync(string cep)
    {
        var normalizedCep = cep.Replace("-", "").Trim();
        using var response = await httpClient.GetAsync($"ws/{normalizedCep}/json/");

        if (!response.IsSuccessStatusCode)
            return null;

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var root = document.RootElement;

        if (root.TryGetProperty("erro", out var erro))
        {
            if (
                (erro.ValueKind == JsonValueKind.True) ||
                (erro.ValueKind == JsonValueKind.String &&
                 erro.GetString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
            )
            {
                return null;
            }
        }

        return new RespostaConsultaEndereco
        {
            Cep = GetStringOrEmpty(root, "cep"),
            Logradouro = GetStringOrEmpty(root, "logradouro"),
            Bairro = GetStringOrEmpty(root, "bairro"),
            Cidade = GetStringOrEmpty(root, "localidade"),
            Estado = GetStringOrEmpty(root, "uf")
        };
    }
}

