using Microsoft.AspNetCore.Mvc;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsuariosController(IUserService servicoUsuario) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListarTodos()
    {
        return Ok(servicoUsuario.ListarTodos());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarPorId(Guid id)
    {
        var usuario = servicoUsuario.BuscarPorId(id);

        return usuario is null
            ? NotFound()
            : Ok(usuario);
    }

    [HttpPost("cadastro")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Cadastrar([FromBody] RequisicaoUsuario requisicao)
    {
        var usuario = servicoUsuario.Cadastrar(requisicao);

        return usuario is null
            ? BadRequest("E-mail ja cadastrado.")
            : Created(string.Empty, usuario);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] RequisicaoLogin requisicao)
    {
        var usuario = servicoUsuario.Login(requisicao);

        return usuario is null
            ? Unauthorized("E-mail ou senha invalidos.")
            : Ok(usuario);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Atualizar(Guid id, [FromBody] RequisicaoAtualizarUsuario requisicao)
    {
        var usuario = servicoUsuario.Atualizar(id, requisicao);

        return usuario is null
            ? NotFound()
            : Ok(usuario);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Excluir(Guid id)
    {
        var excluido = servicoUsuario.Excluir(id);

        return excluido
            ? NoContent()
            : NotFound();
    }
}
