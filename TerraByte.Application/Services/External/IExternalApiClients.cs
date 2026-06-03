using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.External;

public interface IAddressLookupClient
{
    Task<AddressLookupResponse?> FetchAddressAsync(string cep);
}

public interface IGeocodingClient
{
    Task<GeocodeResponse?> FetchCoordinatesAsync(string location);
}

public interface IClimateClient
{
    Task<ClimateForecastResponse> FetchClimateAsync(double latitude, double longitude, int days);
}

public interface ISoilClient
{
    Task<SoilClassificationResponse> FetchSoilAsync(double latitude, double longitude);
}
