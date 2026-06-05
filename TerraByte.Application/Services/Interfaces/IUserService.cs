using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface IUserService
{
    IReadOnlyCollection<RespostaUsuario> ListarTodos();
    RespostaUsuario? BuscarPorId(Guid id);
    RespostaUsuario? Cadastrar(RequisicaoUsuario requisicao);
    RespostaUsuario? Login(RequisicaoLogin requisicao);
    RespostaUsuario? Atualizar(Guid id, RequisicaoAtualizarUsuario requisicao);
    bool Excluir(Guid id);
}
