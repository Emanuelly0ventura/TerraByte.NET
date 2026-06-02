using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class PestOccurrencesController(IPestOccurrenceService pestOccurrenceService) : ControllerBase
{
    //Listar ocorrências de pragas por terreno
    [HttpGet("farm-plots/{farmPlotId:guid}/pest-occurrences")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchByFarmPlot(Guid farmPlotId)
    {
        return Ok(pestOccurrenceService.FetchByFarmPlot(farmPlotId));
    }

    // Buscar uma ocorrência de praga por um id
    [HttpGet("pest-occurrences/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchById(Guid id)
    {
        var occurrence = pestOccurrenceService.FetchById(id);
        return occurrence is null ? NotFound() : Ok(occurrence);
    }

    //Cadastrar uma ocorrência de praga em um terreno
    [HttpPost("farm-plots/{farmPlotId:guid}/pest-occurrences")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Create(Guid farmPlotId, PestOccurrenceRequest request)
    {
        var created = pestOccurrenceService.Create(farmPlotId, request);
        return created is null
            ? NotFound()
            : CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }

    // Remover uma ocorrência de praga
    [HttpDelete("pest-occurrences/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return pestOccurrenceService.Delete(id) ? NoContent() : NotFound();
    }
}
