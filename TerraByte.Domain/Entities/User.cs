namespace TerraByte.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Genero { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? FotoPerfil { get; set; }
    public ICollection<TerrenoAgricola> TerrenosAgricolas { get; set; }
        = new List<TerrenoAgricola>();
}
