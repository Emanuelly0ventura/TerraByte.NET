using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class CropRepository(TerraByteContext context) : Repository<Crop>(context), ICropRepository
{
    public IReadOnlyCollection<Crop> FetchByFarmPlot(Guid farmPlotId)
    {
        return Context.Crops
            .Where(x => x.FarmPlotId == farmPlotId)
            .ToList();
    }
}
