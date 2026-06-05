using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TerrenosAgricolasController(ITerrenoAgricolaService servicoTerrenoAgricola) : ControllerBase
{
    //Listar todos os terrenos cadastradosr
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListarTodos()
    {
        return Ok(servicoTerrenoAgricola.ListarTodos());
    }

    // Busca um terreno por id
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarPorId(Guid id)
    {
        var terreno = servicoTerrenoAgricola.BuscarPorId(id);
        return terreno is null ? NotFound() : Ok(terreno);
    }

    // Cadastrar um terreno agrÃ­cola
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(TerrenoAgricolaDtos requisicao)
    {
        var criado = await servicoTerrenoAgricola.CriarAsync(requisicao);
        return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
    }

    //Atualizar dados principais de um terreno
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AtualizarParcial(Guid id, RequisicaoAtualizarTerrenoAgricola requisicao)
    {
        var atualizado = servicoTerrenoAgricola.AtualizarParcial(id, requisicao);
        return atualizado is null ? NotFound() : Ok(atualizado);
    }

    // Remove um terreno cadastrado.
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Excluir(Guid id)
    {
        return servicoTerrenoAgricola.Excluir(id) ? NoContent() : NotFound();
    }
}

