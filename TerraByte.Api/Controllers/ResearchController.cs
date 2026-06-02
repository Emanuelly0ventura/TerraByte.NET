using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ResearchController(IResearchService researchService) : ControllerBase
{
    // Consultar endereço por CEP
    [HttpGet("cep/{cep}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchAddress(string cep)
    {
        var address = await researchService.FetchAddressAsync(cep);
        return address is null ? NotFound() : Ok(address);
    }

    //Consultar latitude e longitude por cidade 
    [HttpGet("geocode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchCoordinates([FromQuery] string city)
    {
        var coordinates = await researchService.FetchCoordinatesAsync(city);
        return coordinates is null ? NotFound() : Ok(coordinates);
    }

    // Consultar previsão climática de 1 a 30 dias
    [HttpGet("climate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FetchClimate([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] int days = 30)
    {
        var climate = await researchService.FetchClimateAsync(latitude, longitude, days);
        return Ok(climate);
    }

    //Consultar propriedades do solo 
    [HttpGet("soil")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> FetchSoil([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] string property = "clay")
    {
        var soil = await researchService.FetchSoilAsync(latitude, longitude, property);
        return Ok(soil);
    }

    /// Salvar uma pesquisa climática vinculada a um terreno
    [HttpPost("farm-plots/{farmPlotId:guid}/climate-snapshots")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SaveClimateSnapshot(Guid farmPlotId, [FromQuery] int days = 30)
    {
        var snapshot = await researchService.SaveClimateSnapshotAsync(farmPlotId, days);
        return snapshot is null ? NotFound() : Created(string.Empty, snapshot);
    }

    //Salvar uma pesquisa de solo vinculada a um terreno
    [HttpPost("farm-plots/{farmPlotId:guid}/soil-snapshots")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SaveSoilSnapshot(Guid farmPlotId, [FromQuery] string property = "clay")
    {
        var snapshot = await researchService.SaveSoilSnapshotAsync(farmPlotId, property);
        return snapshot is null ? NotFound() : Created(string.Empty, snapshot);
    }
}
