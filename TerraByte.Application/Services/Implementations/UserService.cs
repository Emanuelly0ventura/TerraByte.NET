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
        ValidarUsuario(requisicao.Nome, requisicao.Email, requisicao.Senha, requisicao.Telefone);

        var email = NormalizarEmail(requisicao.Email);
        var usuarioExistente = repositorioUsuario.BuscarPorEmail(email);

        if (usuarioExistente is not null)
            return null;

        var usuario = new Usuario
        {
            Nome = requisicao.Nome.Trim(),
            Email = email,
            Senha = BCrypt.Net.BCrypt.HashPassword(requisicao.Senha),
            Telefone = LimparTextoOpcional(requisicao.Telefone),
            Genero = LimparTextoOpcional(requisicao.Genero),
            DataNascimento = requisicao.DataNascimento.ToDateTime(TimeOnly.MinValue),
            FotoPerfil = requisicao.FotoPerfil
        };

        repositorioUsuario.Criar(usuario);
        repositorioUsuario.SalvarAlteracoes();

        return RespostaUsuario.DoDominio(usuario);
    }

    public RespostaUsuario? Login(RequisicaoLogin requisicao)
    {
        var usuario = repositorioUsuario.BuscarPorEmail(
            NormalizarEmail(requisicao.Email));

        if (usuario is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(
                requisicao.Senha,
                usuario.Senha))
            return null;

        return RespostaUsuario.DoDominio(usuario);
    }

    public RespostaUsuario? Atualizar(Guid id, RequisicaoAtualizarUsuario requisicao)
    {
        var usuario = repositorioUsuario.BuscarPorId(id);

        if (usuario is null)
            return null;

        ValidarUsuario(requisicao.Nome, requisicao.Email, requisicao.Senha, requisicao.Telefone, senhaObrigatoria: false);

        var email = NormalizarEmail(requisicao.Email);
        var usuarioComMesmoEmail = repositorioUsuario.BuscarPorEmail(email);
        if (usuarioComMesmoEmail is not null && usuarioComMesmoEmail.Id != id)
            throw new ArgumentException("E-mail ja cadastrado para outro usuario.");

        usuario.Nome = requisicao.Nome.Trim();
        usuario.Email = email;
        usuario.Telefone = LimparTextoOpcional(requisicao.Telefone);
        usuario.Genero = LimparTextoOpcional(requisicao.Genero);
        usuario.DataNascimento = requisicao.DataNascimento.ToDateTime(TimeOnly.MinValue);
        usuario.FotoPerfil = requisicao.FotoPerfil;

        if (!string.IsNullOrWhiteSpace(requisicao.Senha))
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(requisicao.Senha);
        }

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

    private static void ValidarUsuario(
        string nome,
        string email,
        string senha,
        string? telefone,
        bool senhaObrigatoria = true)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Trim().Length < 3)
            throw new ArgumentException("O nome deve ter no minimo 3 letras.");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("O e-mail e obrigatorio e deve conter @.");

        if (senhaObrigatoria && (string.IsNullOrWhiteSpace(senha) || senha.Length < 8))
            throw new ArgumentException("A senha deve ter no minimo 8 caracteres.");

        if (!senhaObrigatoria && !string.IsNullOrWhiteSpace(senha) && senha.Length < 8)
            throw new ArgumentException("A senha deve ter no minimo 8 caracteres.");

        if (!string.IsNullOrWhiteSpace(telefone) && telefone.Trim().Length < 11)
            throw new ArgumentException("O telefone deve ter no minimo 11 caracteres.");
    }

    private static string NormalizarEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static string? LimparTextoOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
