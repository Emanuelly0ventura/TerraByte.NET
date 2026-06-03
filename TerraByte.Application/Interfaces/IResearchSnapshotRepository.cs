using TerraByte.Domain.Entities;

namespace TerraByte.Application.Interfaces;

public interface IResearchSnapshotRepository : IRepository<ResearchSnapshot>
{
    IReadOnlyCollection<ResearchSnapshot> FetchByFarmPlot(Guid farmPlotId);
}
