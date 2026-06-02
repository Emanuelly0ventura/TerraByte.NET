using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FarmPlotsController(IFarmPlotService farmPlotService) : ControllerBase
{
    //Listar todos os terrenos cadastradosr
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult FetchAll()
    {
        return Ok(farmPlotService.FetchAll());
    }

    // Busca um terreno por id
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult FetchById(Guid id)
    {
        var plot = farmPlotService.FetchById(id);
        return plot is null ? NotFound() : Ok(plot);
    }

    // Cadastrar um terreno agrícola
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(FarmPlotRequest request)
    {
        var created = await farmPlotService.CreateAsync(request);
        return CreatedAtAction(nameof(FetchById), new { id = created.Id }, created);
    }

    //Atualizar dados principais de um terreno
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(Guid id, FarmPlotUpdateRequest request)
    {
        var updated = farmPlotService.Patch(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    // Remove um terreno cadastrado.
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return farmPlotService.Delete(id) ? NoContent() : NotFound();
    }
}
