using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class CropService(
    ICropRepository cropRepository,
    IFarmPlotRepository farmPlotRepository) : ICropService
{
    public IReadOnlyCollection<CropResponse> FetchByFarmPlot(Guid farmPlotId)
    {
        return cropRepository.FetchByFarmPlot(farmPlotId)
            .Select(CropResponse.FromDomain)
            .ToList();
    }

    public CropResponse? FetchById(Guid id)
    {
        var crop = cropRepository.FetchById(id);
        return crop is null ? null : CropResponse.FromDomain(crop);
    }

    public CropResponse? Create(Guid farmPlotId, CropRequest request)
    {
        if (farmPlotRepository.FetchById(farmPlotId) is null)
            return null;

        var crop = new Crop
        {
            FarmPlotId = farmPlotId,
            Name = request.Name.Trim(),
            SeedName = request.SeedName.Trim(),
            PlantingDate = request.PlantingDate,
            Notes = request.Notes.Trim()
        };

        cropRepository.Create(crop);
        cropRepository.SaveChanges();

        return CropResponse.FromDomain(crop);
    }

    public bool Delete(Guid id)
    {
        var crop = cropRepository.FetchById(id);
        if (crop is null)
            return false;

        cropRepository.Delete(crop);
        cropRepository.SaveChanges();
        return true;
    }
}
