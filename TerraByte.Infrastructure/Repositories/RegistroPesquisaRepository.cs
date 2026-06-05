using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class RegistroPesquisaRepository(TerraByteContext context)
    : Repository<RegistroPesquisa>(context), IRegistroPesquisaRepository
{
    public IReadOnlyCollection<RegistroPesquisa> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId)
    {
        return Context.RegistrosPesquisa
            .Where(x => x.TerrenoAgricolaId == terrenoAgricolaId)
            .ToList();
    }
}


