using System.ComponentModel.DataAnnotations;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class RequisicaoUsuario
{
    [Required(ErrorMessage = "O nome e obrigatorio.")]
    [MinLength(3, ErrorMessage = "O nome deve ter no minimo 3 letras.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail e obrigatorio.")]
    [EmailAddress(ErrorMessage = "O e-mail deve conter um formato valido, incluindo @.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha e obrigatoria.")]
    [MinLength(8, ErrorMessage = "A senha deve ter no minimo 8 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [MinLength(11, ErrorMessage = "O telefone deve ter no minimo 11 caracteres.")]
    public string? Telefone { get; set; }

    public string? Genero { get; set; }

    [Required(ErrorMessage = "A data de nascimento e obrigatoria.")]
    public DateOnly DataNascimento { get; set; }

    public string? FotoPerfil { get; set; }
}

public class RequisicaoLogin
{
    [Required(ErrorMessage = "O e-mail e obrigatorio.")]
    [EmailAddress(ErrorMessage = "O e-mail deve conter um formato valido, incluindo @.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha e obrigatoria.")]
    public string Senha { get; set; } = string.Empty;
}

public class RequisicaoAtualizarUsuario
{
    [Required(ErrorMessage = "O nome e obrigatorio.")]
    [MinLength(3, ErrorMessage = "O nome deve ter no minimo 3 letras.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail e obrigatorio.")]
    [EmailAddress(ErrorMessage = "O e-mail deve conter um formato valido, incluindo @.")]
    public string Email { get; set; } = string.Empty;

    [MinLength(8, ErrorMessage = "A senha deve ter no minimo 8 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [MinLength(11, ErrorMessage = "O telefone deve ter no minimo 11 caracteres.")]
    public string? Telefone { get; set; }

    public string? Genero { get; set; }

    [Required(ErrorMessage = "A data de nascimento e obrigatoria.")]
    public DateOnly DataNascimento { get; set; }

    public string? FotoPerfil { get; set; }
}

public class RespostaUsuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Genero { get; set; }
    public DateOnly DataNascimento { get; set; }
    public string? FotoPerfil { get; set; }

    public static RespostaUsuario DoDominio(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Email = usuario.Email,
        Telefone = usuario.Telefone,
        Genero = usuario.Genero,
        DataNascimento = DateOnly.FromDateTime(usuario.DataNascimento),
        FotoPerfil = usuario.FotoPerfil
    };
}
