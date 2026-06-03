using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class ResearchSnapshotRepository(TerraByteContext context)
    : Repository<ResearchSnapshot>(context), IResearchSnapshotRepository
{
    public IReadOnlyCollection<ResearchSnapshot> FetchByFarmPlot(Guid farmPlotId)
    {
        return Context.ResearchSnapshots
            .Where(x => x.FarmPlotId == farmPlotId)
            .ToList();
    }
}
