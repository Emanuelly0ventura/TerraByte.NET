using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api/pesquisas")]
[Produces("application/json")]
public class PesquisasController(IRegistroPesquisaService servicoPesquisa) : ControllerBase
{
    [HttpGet("clima")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuscarClima([FromQuery] double latitude, [FromQuery] double longitude)
    {
        var clima = await servicoPesquisa.BuscarClimaAsync(latitude, longitude, 30);
        return Ok(clima);
    }
}
