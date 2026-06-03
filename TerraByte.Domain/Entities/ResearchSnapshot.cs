namespace TerraByte.Domain.Entities;

public class ResearchSnapshot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Source { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public Guid FarmPlotId { get; set; }
    public FarmPlot? FarmPlot { get; set; }
}
