using TerraByte.Domain.Entities;

namespace TerraByte.Application.Interfaces;

public interface IRegistroPesquisaRepository : IRepository<RegistroPesquisa>
{
    IReadOnlyCollection<RegistroPesquisa> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId);
}

