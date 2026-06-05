using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class CulturasController(ICulturaService servicoCultura) : ControllerBase
{
    [HttpGet("plantios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListarPlantios()
    {
        return Ok(servicoCultura.ListarPlantios());
    }

    [HttpGet("plantios/{plantioId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarPlantioPorId(Guid plantioId)
    {
        var plantio = servicoCultura.BuscarPlantioPorId(plantioId);
        return plantio is null ? NotFound() : Ok(plantio);
    }

    [HttpPost("terrenos-agricolas/{terrenoAgricolaId:guid}/plantios/{plantioId:guid}/analise")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AnalisarCompatibilidade(Guid terrenoAgricolaId, Guid plantioId)
    {
        var analise = await servicoCultura.AnalisarCompatibilidadeAsync(terrenoAgricolaId, plantioId);
        return analise is null ? NotFound() : Ok(analise);
    }
}
