using TerraByte.Domain.Entities;
using System.Text.Json.Serialization;

namespace TerraByte.Application.DTOs;

public class FarmPlotRequest
{
    [JsonPropertyName("nome")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;
}

public class FarmPlotUpdateRequest
{
    [JsonPropertyName("nome")]
    public string Name { get; set; } = string.Empty;
}

public class FarmPlotResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nome")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")]
    public string Street { get; set; } = string.Empty;

    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("cidade")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("nomeSolo")]
    public string SoilName { get; set; } = string.Empty;

    [JsonPropertyName("argila")]
    public double Clay { get; set; }

    [JsonPropertyName("areia")]
    public double Sand { get; set; }

    [JsonPropertyName("silto")]
    public double Silt { get; set; }

    [JsonPropertyName("raioSoloKm")]
    public double SoilRadiusKm { get; set; }

    public static FarmPlotResponse FromDomain(FarmPlot plot) => new()
    {
        Id = plot.Id,
        Name = plot.Name,
        Cep = plot.Cep,
        Street = plot.Street,
        City = plot.City,
        State = plot.State,
        Latitude = plot.Latitude,
        Longitude = plot.Longitude,
        SoilName = plot.SoilName,
        Clay = plot.Clay,
        Sand = plot.Sand,
        Silt = plot.Silt,
        SoilRadiusKm = plot.SoilRadiusKm
    };
}
