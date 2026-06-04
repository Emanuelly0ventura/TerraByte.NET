using TerraByte.Aplicacao.Interfaces;
using TerraByte.Dominio.Entidades;
using TerraByte.Infraestrutura.Persistencia;

namespace TerraByte.Infraestrutura.Repositorios;

public class RepositorioCultura(TerraByteContext context) : Repositorio<Cultura>(context), IRepositorioCultura
{
    public IReadOnlyCollection<Cultura> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId)
    {
        return Context.Culturas
            .Where(x => x.TerrenoAgricolaId == terrenoAgricolaId)
            .ToList();
    }
}


