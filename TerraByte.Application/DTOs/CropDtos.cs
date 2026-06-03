using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class CropRequest
{
    public string Name { get; set; } = string.Empty;
    public string SeedName { get; set; } = string.Empty;
    public DateOnly PlantingDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CropResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SeedName { get; set; } = string.Empty;
    public DateOnly PlantingDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid FarmPlotId { get; set; }

    public static CropResponse FromDomain(Crop crop) => new()
    {
        Id = crop.Id,
        Name = crop.Name,
        SeedName = crop.SeedName,
        PlantingDate = crop.PlantingDate,
        Notes = crop.Notes,
        FarmPlotId = crop.FarmPlotId
    };
}
