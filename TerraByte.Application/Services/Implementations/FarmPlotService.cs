using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class FarmPlotService(
    IFarmPlotRepository farmPlotRepository,
    IAddressLookupClient addressLookupClient,
    IGeocodingClient geocodingClient,
    ISoilClient soilClient) : IFarmPlotService
{
    public IReadOnlyCollection<FarmPlotResponse> FetchAll()
    {
        return farmPlotRepository.FetchAll()
            .Select(FarmPlotResponse.FromDomain)
            .ToList();
    }

    public FarmPlotResponse? FetchById(Guid id)
    {
        var plot = farmPlotRepository.FetchById(id);
        return plot is null ? null : FarmPlotResponse.FromDomain(plot);
    }

    public async Task<FarmPlotResponse> CreateAsync(FarmPlotRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("O nome do terreno deve ser informado.");

        if (string.IsNullOrWhiteSpace(request.Cep))
            throw new ArgumentException("O CEP do terreno deve ser informado.");

        var address = await addressLookupClient.FetchAddressAsync(request.Cep)
            ?? throw new ArgumentException("CEP não encontrado no ViaCEP.");

        var coordinates = await FetchCoordinatesAsync(address)
            ?? throw new ArgumentException("Não foi possível encontrar latitude e longitude para o CEP informado.");

        var soil = await soilClient.FetchSoilAsync(coordinates.Latitude, coordinates.Longitude);

        var plot = new FarmPlot
        {
            Name = request.Name.Trim(),
            Cep = address.Cep,
            Latitude = coordinates.Latitude,
            Longitude = coordinates.Longitude,
            SoilName = soil.SoilName,
            Clay = soil.Clay,
            Sand = soil.Sand,
            Silt = soil.Silt,
            SoilRadiusKm = soil.SoilRadiusKm,
            Street = address.Street,
            District = address.District,
            City = address.City,
            State = address.State
        };

        farmPlotRepository.Create(plot);
        farmPlotRepository.SaveChanges();

        return FarmPlotResponse.FromDomain(plot);
    }

    public FarmPlotResponse? Patch(Guid id, FarmPlotUpdateRequest request)
    {
        var plot = farmPlotRepository.FetchById(id);
        if (plot is null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.Name))
            plot.Name = request.Name.Trim();

        farmPlotRepository.Patch(plot);
        farmPlotRepository.SaveChanges();

        return FarmPlotResponse.FromDomain(plot);
    }

    public bool Delete(Guid id)
    {
        var plot = farmPlotRepository.FetchById(id);
        if (plot is null)
            return false;

        farmPlotRepository.Delete(plot);
        farmPlotRepository.SaveChanges();
        return true;
    }

    private async Task<GeocodeResponse?> FetchCoordinatesAsync(AddressLookupResponse address)
    {
        foreach (var location in BuildLocationQueries(address))
        {
            var coordinates = await geocodingClient.FetchCoordinatesAsync(location);
            if (coordinates is not null)
                return coordinates;
        }

        return null;
    }

    private static IEnumerable<string> BuildLocationQueries(AddressLookupResponse address)
    {
        var fullAddress = JoinLocationParts(address.Street, address.District, address.City, address.State, "Brasil");
        if (!string.IsNullOrWhiteSpace(fullAddress))
            yield return fullAddress;

        var cityAddress = JoinLocationParts(address.City, address.State, "Brasil");
        if (!string.IsNullOrWhiteSpace(cityAddress) && cityAddress != fullAddress)
            yield return cityAddress;
    }

    private static string JoinLocationParts(params string[] parts)
    {
        return string.Join(", ", parts.Where(part => !string.IsNullOrWhiteSpace(part)));
    }
}
