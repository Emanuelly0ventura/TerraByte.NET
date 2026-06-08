namespace TerraByte.Domain.Entities;

public class RegistroPesquisa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public double UmidadeMed { get; set; }
    public double ChuvaPrevistaMm { get; set; }
    public double AdequadoPlantio { get; set; }
    public string NivelRisco { get; set; } = string.Empty;
    public string Recomendacao { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public Guid TerrenoAgricolaId { get; set; }
    public TerrenoAgricola? TerrenoAgricola { get; set; }
    public Guid CulturaId { get; set; }
    public Cultura? Cultura { get; set; }
}
