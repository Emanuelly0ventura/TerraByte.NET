using TerraByte.Aplicacao.Dtos;

namespace TerraByte.Aplicacao.Servicos.Interfaces;

public interface IServicoCultura
{
    IReadOnlyCollection<RespostaPlantio> ListarPlantios();
    RespostaPlantio? BuscarPlantioPorId(Guid plantioId);
    Task<RespostaAnalisePlantio?> AnalisarCompatibilidadeAsync(Guid terrenoAgricolaId, Guid plantioId);
}
