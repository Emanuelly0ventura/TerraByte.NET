using System.Text.Json;
using TerraByte.Aplicacao.Dtos;
using TerraByte.Aplicacao.Servicos.Externo;

namespace TerraByte.Infraestrutura.Externo;

public class ClienteEnderecoViaCep(HttpClient httpClient) : IClienteConsultaEndereco
{
    public async Task<RespostaConsultaEndereco?> BuscarEnderecoAsync(string cep)
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

        return new RespostaConsultaEndereco
        {
            Cep = root.GetProperty("cep").GetString() ?? normalizedCep,
            Logradouro = root.GetProperty("logradouro").GetString() ?? string.Empty,
            Bairro = root.GetProperty("bairro").GetString() ?? string.Empty,
            Cidade = root.GetProperty("localidade").GetString() ?? string.Empty,
            Estado = root.GetProperty("uf").GetString() ?? string.Empty
        };
    }
}

