using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class ResearchSnapshotResponse
{
    public Guid Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
    public Guid FarmPlotId { get; set; }

    public static ResearchSnapshotResponse FromDomain(ResearchSnapshot snapshot) => new()
    {
        Id = snapshot.Id,
        Source = snapshot.Source,
        Kind = snapshot.Kind,
        Summary = snapshot.Summary,
        RequestedAt = snapshot.RequestedAt,
        FarmPlotId = snapshot.FarmPlotId
    };
}
