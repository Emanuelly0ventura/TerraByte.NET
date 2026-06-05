using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class CulturaRepository(TerraByteContext context) : Repository<Cultura>(context), ICulturaRepository
{
    public IReadOnlyCollection<Cultura> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId)
    {
        return Context.Culturas
            .Where(x => x.TerrenoAgricolaId == terrenoAgricolaId)
            .ToList();
    }
}


