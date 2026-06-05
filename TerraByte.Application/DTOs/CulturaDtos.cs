using System.Text.Json.Serialization;

namespace TerraByte.Application.DTOs;

public enum TipoSoloEnum
{
    AREIA,
    AREIA_FRANCA,
    FRANCO_ARENOSO,
    FRANCA,
    FRANCO_SILTOSA,
    SILTE,
    FRANCO_ARGILO_ARENOSA,
    FRANCO_ARGILOSA,
    FRANCO_ARGILO_SILTOSA,
    ARGILO_ARENOSA,
    ARGILA,
    ARGILO_SILTOSA,
    MUITO_ARGILOSA,
    DESCONHECIDO
}

public class RespostaPlantio
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("tempMin")]
    public double TempMin { get; set; }

    [JsonPropertyName("tempMax")]
    public double TempMax { get; set; }

    [JsonPropertyName("aguaMM")]
    public double AguaMM { get; set; }

    [JsonPropertyName("mesesIdeais")]
    public IReadOnlyCollection<string> MesesIdeais { get; set; } = [];

    [JsonPropertyName("urlImg")]
    public string UrlImg { get; set; } = string.Empty;

    [JsonPropertyName("tiposSolo")]
    public IReadOnlyCollection<string> TiposSolo { get; set; } = [];

    [JsonPropertyName("defensivos")]
    public IReadOnlyCollection<string> Defensivos { get; set; } = [];
}

public class RespostaAnalisePlantio
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonPropertyName("data")]
    public DateTime Data { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("nomeEndereco")]
    public string NomeEndereco { get; set; } = string.Empty;

    [JsonPropertyName("nomePlantio")]
    public string NomePlantio { get; set; } = string.Empty;

    [JsonPropertyName("tipoSoloEndereco")]
    public string TipoSoloEndereco { get; set; } = string.Empty;

    [JsonPropertyName("tipoSoloPlantio")]
    public IReadOnlyCollection<string> TipoSoloPlantio { get; set; } = [];

    [JsonPropertyName("adequadoPlantio")]
    public string AdequadoPlantio { get; set; } = string.Empty;

    [JsonPropertyName("nivelRisco")]
    public string NivelRisco { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("argila")]
    public double Argila { get; set; }

    [JsonPropertyName("areia")]
    public double Areia { get; set; }

    [JsonPropertyName("silte")]
    public double Silte { get; set; }

    [JsonPropertyName("raioKM")]
    public double RaioKm { get; set; }

    [JsonPropertyName("tempMin")]
    public string TempMin { get; set; } = string.Empty;

    [JsonPropertyName("tempMax")]
    public string TempMax { get; set; } = string.Empty;

    [JsonPropertyName("umidadeMed")]
    public string UmidadeMed { get; set; } = string.Empty;

    [JsonPropertyName("recomendacao")]
    public string Recomendacao { get; set; } = string.Empty;

    [JsonPropertyName("pontuacao")]
    public int Pontuacao { get; set; }
}
