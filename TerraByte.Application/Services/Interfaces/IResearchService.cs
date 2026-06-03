using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface IResearchService
{
    Task<AddressLookupResponse?> FetchAddressAsync(string cep);
    Task<GeocodeResponse?> FetchCoordinatesAsync(string city);
    Task<ClimateForecastResponse> FetchClimateAsync(double latitude, double longitude, int days);
    Task<SoilClassificationResponse> FetchSoilAsync(double latitude, double longitude);
    Task<ResearchSnapshotResponse?> SaveClimateSnapshotAsync(Guid farmPlotId, int days);
    Task<ResearchSnapshotResponse?> SaveSoilSnapshotAsync(Guid farmPlotId);
}
