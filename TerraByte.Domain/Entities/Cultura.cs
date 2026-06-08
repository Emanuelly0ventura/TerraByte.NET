namespace TerraByte.Domain.Entities;

public class Cultura
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public double AguaMM { get; set; }
    public string MesesIdeais { get; set; } = string.Empty;
    public string UrlImg { get; set; } = string.Empty;
    public ICollection<TipoSolo> TiposSolo { get; set; } = new List<TipoSolo>();
    public ICollection<Defensivo> Defensivos { get; set; } = new List<Defensivo>();
    public ICollection<RegistroPesquisa> RegistrosPesquisa { get; set; } = new List<RegistroPesquisa>();
}


