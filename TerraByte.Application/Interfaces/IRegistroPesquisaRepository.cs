using TerraByte.Dominio.Entidades;

namespace TerraByte.Aplicacao.Interfaces;

public interface IRepositorioRegistroPesquisa : IRepositorio<RegistroPesquisa>
{
    IReadOnlyCollection<RegistroPesquisa> BuscarPorTerrenoAgricola(Guid terrenoAgricolaId);
}

