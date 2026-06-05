namespace TerraByte.Domain.Entities;

public class RegistroPesquisa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Fonte { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Resumo { get; set; } = string.Empty;
    public DateTime SolicitadoEm { get; set; } = DateTime.UtcNow;
    public Guid TerrenoAgricolaId { get; set; }
    public TerrenoAgricola? TerrenoAgricola { get; set; }
}


