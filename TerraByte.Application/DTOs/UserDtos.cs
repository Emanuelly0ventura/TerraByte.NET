using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class RequisicaoUsuario
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Genero { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? FotoPerfil { get; set; }
}

public class RequisicaoLogin
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class RequisicaoAtualizarUsuario
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Genero { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? FotoPerfil { get; set; }
}

public class RespostaUsuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Genero { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? FotoPerfil { get; set; }

    public static RespostaUsuario DoDominio(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Email = usuario.Email,
        Telefone = usuario.Telefone,
        Genero = usuario.Genero,
        DataNascimento = usuario.DataNascimento,
        FotoPerfil = usuario.FotoPerfil
    };
}
