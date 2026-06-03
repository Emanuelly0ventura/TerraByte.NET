using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface ICropService
{
    IReadOnlyCollection<CropResponse> FetchByFarmPlot(Guid farmPlotId);
    CropResponse? FetchById(Guid id);
    CropResponse? Create(Guid farmPlotId, CropRequest request);
    bool Delete(Guid id);
}
