using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class TerrenoAgricolaRepository(TerraByteContext context)
    : Repository<TerrenoAgricola>(context), ITerrenoAgricolaRepository
{
    public override IReadOnlyCollection<TerrenoAgricola> ListarTodos()
    {
        return Context.TerrenosAgricolas
            .Include(x => x.Culturas)
            .ToList();
    }

    public override TerrenoAgricola? BuscarPorId(Guid id)
    {
        return Context.TerrenosAgricolas
            .Include(x => x.Culturas)
            .Include(x => x.RegistrosPesquisa)
            .FirstOrDefault(x => x.Id == id);
    }
}

