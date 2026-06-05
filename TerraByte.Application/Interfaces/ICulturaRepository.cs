using TerraByte.Domain.Entities;

namespace TerraByte.Application.Interfaces;

public interface ICulturaRepository : IRepository<Cultura>
{
    IReadOnlyCollection<Cultura> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId);
}

