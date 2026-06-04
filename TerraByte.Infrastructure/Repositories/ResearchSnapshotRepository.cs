using TerraByte.Aplicacao.Interfaces;
using TerraByte.Dominio.Entidades;
using TerraByte.Infraestrutura.Persistencia;

namespace TerraByte.Infraestrutura.Repositorios;

public class RepositorioRegistroPesquisa(TerraByteContext context)
    : Repositorio<RegistroPesquisa>(context), IRepositorioRegistroPesquisa
{
    public IReadOnlyCollection<RegistroPesquisa> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId)
    {
        return Context.RegistrosPesquisa
            .Where(x => x.TerrenoAgricolaId == terrenoAgricolaId)
            .ToList();
    }
}


