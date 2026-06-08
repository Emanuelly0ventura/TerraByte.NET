using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface IDefensivoService
{
    public abstract IReadOnlyCollection<DefensivoDtos.RespostaDefensivo> ListarDefensivos();
    public abstract DefensivoDtos.RespostaDefensivo? BuscarDefensivoPorId(Guid DefensivoId);
}