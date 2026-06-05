using TerraByte.Aplicacao.Dtos;

namespace TerraByte.Aplicacao.Servicos.Interfaces;

public interface IServicoTerrenoAgricola
{
    IReadOnlyCollection<RespostaTerrenoAgricola> ListarTodos();
    RespostaTerrenoAgricola? BuscarPorId(Guid id);
    Task<RespostaTerrenoAgricola> CriarAsync(RequisicaoTerrenoAgricola requisicao);
    RespostaTerrenoAgricola? AtualizarParcial(Guid id, RequisicaoAtualizarTerrenoAgricola requisicao);
    bool Excluir(Guid id);
}

