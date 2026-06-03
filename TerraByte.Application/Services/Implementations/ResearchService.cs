using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class ResearchService(
    IFarmPlotRepository farmPlotRepository,
    IResearchSnapshotRepository researchSnapshotRepository,
    IAddressLookupClient addressLookupClient,
    IGeocodingClient geocodingClient,
    IClimateClient climateClient,
    ISoilClient soilClient) : IResearchService
{
    public Task<AddressLookupResponse?> FetchAddressAsync(string cep)
    {
        return addressLookupClient.FetchAddressAsync(cep);
    }

    public Task<GeocodeResponse?> FetchCoordinatesAsync(string city)
    {
        return geocodingClient.FetchCoordinatesAsync(city);
    }

    public Task<ClimateForecastResponse> FetchClimateAsync(double latitude, double longitude, int days)
    {
        return climateClient.FetchClimateAsync(latitude, longitude, days);
    }

    public Task<SoilClassificationResponse> FetchSoilAsync(double latitude, double longitude)
    {
        return soilClient.FetchSoilAsync(latitude, longitude);
    }

    public async Task<ResearchSnapshotResponse?> SaveClimateSnapshotAsync(Guid farmPlotId, int days)
    {
        var plot = farmPlotRepository.FetchById(farmPlotId);
        if (plot is null || plot.Latitude is null || plot.Longitude is null)
            return null;

        var climate = await climateClient.FetchClimateAsync(plot.Latitude.Value, plot.Longitude.Value, days);
        var snapshot = new ResearchSnapshot
        {
            FarmPlotId = plot.Id,
            Source = "OpenWeather",
            Kind = "Clima",
            Summary = climate.Summary
        };

        researchSnapshotRepository.Create(snapshot);
        researchSnapshotRepository.SaveChanges();

        return ResearchSnapshotResponse.FromDomain(snapshot);
    }

    public async Task<ResearchSnapshotResponse?> SaveSoilSnapshotAsync(Guid farmPlotId)
    {
        var plot = farmPlotRepository.FetchById(farmPlotId);
        if (plot is null || plot.Latitude is null || plot.Longitude is null)
            return null;

        var soil = await soilClient.FetchSoilAsync(plot.Latitude.Value, plot.Longitude.Value);
        var snapshot = new ResearchSnapshot
        {
            FarmPlotId = plot.Id,
            Source = "SoilGrids",
            Kind = "Solo",
            Summary = $"Solo {soil.SoilName}: argila {soil.Clay}%, areia {soil.Sand}% e silte {soil.Silt}% em um raio aproximado de {soil.SoilRadiusKm} km."
        };

        researchSnapshotRepository.Create(snapshot);
        researchSnapshotRepository.SaveChanges();

        return ResearchSnapshotResponse.FromDomain(snapshot);
    }
}
