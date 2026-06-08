using System.Text.Json.Serialization;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class RegistroPesquisaDtos
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("data")]
    public DateTime Data { get; set; }

    [JsonPropertyName("tempMin")]
    public double TempMin { get; set; }

    [JsonPropertyName("tempMax")]
    public double TempMax { get; set; }

    [JsonPropertyName("umidadeMed")]
    public double UmidadeMed { get; set; }

    [JsonPropertyName("chuvaPrevistaMm")]
    public double ChuvaPrevistaMm { get; set; }

    [JsonPropertyName("pontuacao")]
    public double AdequadoPlantio { get; set; }

    [JsonPropertyName("classificacao")]
    public string NivelRisco { get; set; } = string.Empty;

    [JsonPropertyName("recomendacao")]
    public string Recomendacao { get; set; } = string.Empty;

    [JsonPropertyName("usuarioId")]
    public Guid UsuarioId { get; set; }

    [JsonPropertyName("terrenoAgricolaId")]
    public Guid TerrenoAgricolaId { get; set; }

    [JsonPropertyName("culturaId")]
    public Guid CulturaId { get; set; }

    public static RegistroPesquisaDtos DoDominio(RegistroPesquisa registro) => new()
    {
        Id = registro.Id,
        Data = registro.Data,
        TempMin = registro.TempMin,
        TempMax = registro.TempMax,
        UmidadeMed = registro.UmidadeMed,
        ChuvaPrevistaMm = registro.ChuvaPrevistaMm,
        AdequadoPlantio = registro.AdequadoPlantio,
        NivelRisco = registro.NivelRisco,
        Recomendacao = registro.Recomendacao,
        UsuarioId = registro.UsuarioId,
        TerrenoAgricolaId = registro.TerrenoAgricolaId,
        CulturaId = registro.CulturaId
    };
}
