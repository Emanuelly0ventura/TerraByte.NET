using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface ICulturaService
{
    IReadOnlyCollection<RespostaPlantio> ListarPlantios();
    RespostaPlantio? BuscarPlantioPorId(Guid plantioId);
    Task<RespostaAnalisePlantio?> AnalisarCompatibilidadeAsync(Guid terrenoAgricolaId, Guid plantioId);
}
