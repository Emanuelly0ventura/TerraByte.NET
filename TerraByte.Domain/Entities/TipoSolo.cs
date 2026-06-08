namespace TerraByte.Domain.Entities;

public class TipoSolo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public ICollection<Cultura> Culturas { get; set; } = new List<Cultura>();
    public ICollection<TerrenoAgricola> TerrenosAgricolas { get; set; } = new List<TerrenoAgricola>();
}
