using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class DefensivoRepository(TerraByteContext context) : Repository<Defensivo>(context), IDefensivoRepository
{
    public override IReadOnlyCollection<Defensivo> ListarTodos()
    {
        return Context.Defensivos
            .ToList();
    }

    public override Defensivo? BuscarPorId(Guid id)
    {
        return Context.Defensivos
            .FirstOrDefault(x => x.Id == id);
    }
}