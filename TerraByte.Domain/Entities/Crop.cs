namespace TerraByte.Domain.Entities;

public class Crop
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string SeedName { get; set; } = string.Empty;
    public DateOnly PlantingDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid FarmPlotId { get; set; }
    public FarmPlot? FarmPlot { get; set; }
}
