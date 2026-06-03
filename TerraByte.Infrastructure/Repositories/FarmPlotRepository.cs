using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class FarmPlotRepository(TerraByteContext context)
    : Repository<FarmPlot>(context), IFarmPlotRepository
{
    public override IReadOnlyCollection<FarmPlot> FetchAll()
    {
        return Context.FarmPlots
            .Include(x => x.Crops)
            .ToList();
    }

    public override FarmPlot? FetchById(Guid id)
    {
        return Context.FarmPlots
            .Include(x => x.Crops)
            .Include(x => x.ResearchSnapshots)
            .FirstOrDefault(x => x.Id == id);
    }
}
