using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class UserService(
    IUserRepository repositorioUsuario) : IUserService
{
    public IReadOnlyCollection<RespostaUsuario> ListarTodos()
    {
        return repositorioUsuario.ListarTodos()
            .Select(RespostaUsuario.DoDominio)
            .ToList();
    }

    public RespostaUsuario? BuscarPorId(Guid id)
    {
        var usuario = repositorioUsuario.BuscarPorId(id);

        return usuario is null
            ? null
            : RespostaUsuario.DoDominio(usuario);
    }

    public RespostaUsuario? Cadastrar(RequisicaoUsuario requisicao)
    {
        var usuarioExistente = repositorioUsuario.BuscarPorEmail(requisicao.Email);

        if (usuarioExistente is not null)
            return null;

        var usuario = new Usuario
        {
            Nome = requisicao.Nome.Trim(),
            Email = requisicao.Email.Trim().ToLower(),
            Senha = requisicao.Senha,
            Telefone = requisicao.Telefone,
            Genero = requisicao.Genero,
            DataNascimento = requisicao.DataNascimento,
            FotoPerfil = requisicao.FotoPerfil
        };

        repositorioUsuario.Criar(usuario);
        repositorioUsuario.SalvarAlteracoes();

        return RespostaUsuario.DoDominio(usuario);
    }

    public RespostaUsuario? Login(RequisicaoLogin requisicao)
    {
        var usuario = repositorioUsuario.BuscarPorEmail(requisicao.Email);

        if (usuario is null)
            return null;

        if (usuario.Senha != requisicao.Senha)
            return null;

        return RespostaUsuario.DoDominio(usuario);
    }

    public RespostaUsuario? Atualizar(Guid id, RequisicaoAtualizarUsuario requisicao)
    {
        var usuario = repositorioUsuario.BuscarPorId(id);

        if (usuario is null)
            return null;

        usuario.Nome = requisicao.Nome;
        usuario.Email = requisicao.Email;
        usuario.Telefone = requisicao.Telefone;
        usuario.Genero = requisicao.Genero;
        usuario.DataNascimento = requisicao.DataNascimento;
        usuario.FotoPerfil = requisicao.FotoPerfil;

        if (!string.IsNullOrWhiteSpace(requisicao.Senha))
            usuario.Senha = requisicao.Senha;

        repositorioUsuario.SalvarAlteracoes();

        return RespostaUsuario.DoDominio(usuario);
    }

    public bool Excluir(Guid id)
    {
        var usuario = repositorioUsuario.BuscarPorId(id);

        if (usuario is null)
            return false;

        repositorioUsuario.Excluir(usuario);
        repositorioUsuario.SalvarAlteracoes();

        return true;
    }
}
