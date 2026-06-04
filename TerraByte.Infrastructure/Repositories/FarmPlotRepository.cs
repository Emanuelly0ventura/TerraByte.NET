using Microsoft.EntityFrameworkCore;
using TerraByte.Aplicacao.Interfaces;
using TerraByte.Dominio.Entidades;
using TerraByte.Infraestrutura.Persistencia;

namespace TerraByte.Infraestrutura.Repositorios;

public class RepositorioTerrenoAgricola(TerraByteContext context)
    : Repositorio<TerrenoAgricola>(context), IRepositorioTerrenoAgricola
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

