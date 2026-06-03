namespace TerraByte.Domain.Entities;

public class FarmPlot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string SoilName { get; set; } = string.Empty;
    public double Clay { get; set; }
    public double Sand { get; set; }
    public double Silt { get; set; }
    public double SoilRadiusKm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Crop> Crops { get; set; } = [];
    public List<ResearchSnapshot> ResearchSnapshots { get; set; } = [];
}
