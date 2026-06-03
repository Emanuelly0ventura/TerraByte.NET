namespace TerraByte.Application.DTOs;

public class AddressLookupResponse
{
    public string Cep { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public class GeocodeResponse
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class ClimateForecastResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Days { get; set; }
    public string Summary { get; set; } = string.Empty;
}

public class SoilClassificationResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string SoilName { get; set; } = string.Empty;
    public double Clay { get; set; }
    public double Sand { get; set; }
    public double Silt { get; set; }
    public double SoilRadiusKm { get; set; } = 5.55;
}
