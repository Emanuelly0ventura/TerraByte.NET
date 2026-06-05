namespace TerraByte.Dominio.Entidades;

public class Cultura
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string NomeSemente { get; set; } = string.Empty;
    public DateOnly DataPlantio { get; set; }
    public string Observacoes { get; set; } = string.Empty;
    public Guid TerrenoAgricolaId { get; set; }
    public TerrenoAgricola? TerrenoAgricola { get; set; }
}


