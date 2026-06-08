using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class CulturaRepository(TerraByteContext context) : Repository<Cultura>(context), ICulturaRepository
{
    public override IReadOnlyCollection<Cultura> ListarTodos()
    {
        return Context.Culturas
            .Include(x => x.TiposSolo)
            .Include(x => x.Defensivos)
            .AsSplitQuery()
            .ToList();
    }

    public override Cultura? BuscarPorId(Guid id)
    {
        return Context.Culturas
            .Include(x => x.TiposSolo)
            .Include(x => x.Defensivos)
            .AsSplitQuery()
            .FirstOrDefault(x => x.Id == id);
    }
}
