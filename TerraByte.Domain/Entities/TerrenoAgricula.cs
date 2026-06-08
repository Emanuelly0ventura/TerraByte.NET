namespace TerraByte.Domain.Entities;

public class TerrenoAgricola
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string NomeSolo { get; set; } = string.Empty;
    public double Argila { get; set; }
    public double Areia { get; set; }
    public double Silte { get; set; }
    public double RaioSoloKm { get; set; }
    public Guid? TipoSoloId { get; set; }
    public TipoSolo? TipoSolo { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<RegistroPesquisa> RegistrosPesquisa { get; set; } = [];
}


