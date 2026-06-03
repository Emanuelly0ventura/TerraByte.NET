using TerraByte.Domain.Entities;

namespace TerraByte.Application.Interfaces;

public interface ICropRepository : IRepository<Crop>
{
    IReadOnlyCollection<Crop> FetchByFarmPlot(Guid farmPlotId);
}
