using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class CropsController(ICropService cropService) : ControllerBase
{
    //Listar as culturas de um terreno
    [HttpGet("farm-plots/{farmPlotId:guid}/crops")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchByFarmPlot(Guid farmPlotId)
    {
        return Ok(cropService.FetchByFarmPlot(farmPlotId));
    }

    //Buscar uma cultura por id
    [HttpGet("crops/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchById(Guid id)
    {
        var crop = cropService.FetchById(id);
        return crop is null ? NotFound() : Ok(crop);
    }

    //Cadastrar uma cultura em um terreno
    [HttpPost("farm-plots/{farmPlotId:guid}/crops")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Create(Guid farmPlotId, CropRequest request)
    {
        var created = cropService.Create(farmPlotId, request);
        return created is null
            ? NotFound()
            : CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }

    //Remover uma cultura cadastrada
    [HttpDelete("crops/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return cropService.Delete(id) ? NoContent() : NotFound();
    }
}
