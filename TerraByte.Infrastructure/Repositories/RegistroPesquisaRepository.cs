using Microsoft.EntityFrameworkCore;
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
            .Include(x => x.Cultura)
            .Include(x => x.TerrenoAgricola)
            .Where(x => x.TerrenoAgricolaId == terrenoAgricolaId)
            .OrderByDescending(x => x.Data)
            .ToList();
    }
}
