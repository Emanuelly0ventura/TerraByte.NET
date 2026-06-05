using Microsoft.AspNetCore.Mvc;
using TerraByte.Aplicacao.Servicos.Interfaces;

namespace TerraByte.Api.Controladores;

[ApiController]
[Route("api/pesquisas")]
[Produces("application/json")]
public class PesquisasController(IServicoPesquisa servicoPesquisa) : ControllerBase
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
