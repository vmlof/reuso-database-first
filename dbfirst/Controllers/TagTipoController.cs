using dbfirst.Data.Models;
using dbfirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace dbfirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagTipoController : ControllerBase
{
    private readonly TagTipoService _tagTipoService;

    public TagTipoController(TagTipoService tagTipoService)
    {
        _tagTipoService = tagTipoService;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TagTipo dto)
    {
        try
        {
            var resultado = await _tagTipoService.Atualiza(id, dto);
            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TagTipo dto)
    {
        try
        {
            var resultado = await _tagTipoService.Criar(dto);
            return StatusCode(201, resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _tagTipoService.DeletarPorId(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 50, [FromQuery] int offset = 0)
    {
        try
        {
            var resultadoPaginado = await _tagTipoService.ObterTodos(limit, offset);
            return Ok(resultadoPaginado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var resultado = await _tagTipoService.ObterPorId(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }
}