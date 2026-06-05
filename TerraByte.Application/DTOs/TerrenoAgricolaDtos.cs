using System.Text.Json.Serialization;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class TerrenoAgricolaDtos
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("usuarioId")]
    public Guid UsuarioId { get; set; }
}

public class RequisicaoAtualizarTerrenoAgricola
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
}

public class RespostaTerrenoAgricola
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")]
    public string Logradouro { get; set; } = string.Empty;

    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("cidade")]
    public string Cidade { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("nomeSolo")]
    public string NomeSolo { get; set; } = string.Empty;

    [JsonPropertyName("argila")]
    public double Argila { get; set; }

    [JsonPropertyName("areia")]
    public double Areia { get; set; }

    [JsonPropertyName("silto")]
    public double Silte { get; set; }

    [JsonPropertyName("raioSoloKm")]
    public double RaioSoloKm { get; set; }

    public static RespostaTerrenoAgricola DoDominio(TerrenoAgricola terreno) => new()
    {
        Id = terreno.Id,
        Nome = terreno.Nome,
        Cep = terreno.Cep,
        Logradouro = terreno.Logradouro,
        Cidade = terreno.Cidade,
        Estado = terreno.Estado,
        Latitude = terreno.Latitude,
        Longitude = terreno.Longitude,
        NomeSolo = terreno.NomeSolo,
        Argila = terreno.Argila,
        Areia = terreno.Areia,
        Silte = terreno.Silte,
        RaioSoloKm = terreno.RaioSoloKm
    };
}

