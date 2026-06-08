using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api/defensivos")]
[Produces("application/json")]
public class DefensivoController(IDefensivoService servicoDefensivo) : ControllerBase
{
    
    [HttpGet("defensivos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListarDefensivos()
    {
        return Ok(servicoDefensivo.ListarDefensivos());
    }
    
    [HttpGet("defensivos/{defensivoId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarDefensivoPorId(Guid defensivoId)
    {
        var defensivo = servicoDefensivo.BuscarDefensivoPorId(defensivoId);

        return defensivo is null
            ? NotFound()
            : Ok(defensivo);
    }
}