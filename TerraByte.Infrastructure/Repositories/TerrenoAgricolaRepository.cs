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
            .Include(x => x.TipoSolo)
            .Include(x => x.Usuario)
            .ToList();
    }

    public override TerrenoAgricola? BuscarPorId(Guid id)
    {
        return Context.TerrenosAgricolas
            .Include(x => x.TipoSolo)
            .Include(x => x.Usuario)
            .Include(x => x.RegistrosPesquisa)
            .FirstOrDefault(x => x.Id == id);
    }
}
