using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface ITerrenoAgricolaService
{
    IReadOnlyCollection<RespostaTerrenoAgricola> ListarTodos();
    RespostaTerrenoAgricola? BuscarPorId(Guid id);
    Task<RespostaTerrenoAgricola> CriarAsync(TerrenoAgricolaDtos requisicao);
    RespostaTerrenoAgricola? AtualizarParcial(Guid id, RequisicaoAtualizarTerrenoAgricola requisicao);
    bool Excluir(Guid id);
}

