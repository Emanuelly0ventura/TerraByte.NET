namespace TerraByte.Domain.Entities;

public class Defensivo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public ICollection<Cultura> Culturas { get; set; } = new List<Cultura>();
    
    
}
