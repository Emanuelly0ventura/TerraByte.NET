using TerraByte.Dominio.Entidades;

namespace TerraByte.Aplicacao.Interfaces;

public interface IRepositorioCultura : IRepositorio<Cultura>
{
    IReadOnlyCollection<Cultura> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId);
}

