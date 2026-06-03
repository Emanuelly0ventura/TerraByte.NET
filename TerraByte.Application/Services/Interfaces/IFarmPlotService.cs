using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface IFarmPlotService
{
    IReadOnlyCollection<FarmPlotResponse> FetchAll();
    FarmPlotResponse? FetchById(Guid id);
    Task<FarmPlotResponse> CreateAsync(FarmPlotRequest request);
    FarmPlotResponse? Patch(Guid id, FarmPlotUpdateRequest request);
    bool Delete(Guid id);
}
