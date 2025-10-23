using dbfirst.Data.Models;
using dbfirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace dbfirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;

    public TagController(TagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Tag dto)
    {
        try
        {
            var resultado = await _tagService.Criar(dto);
            return Ok(resultado);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Tag dto)
    {
        try
        {
            var resultado = await _tagService.Atualizar(id, dto);
            return Ok(resultado);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _tagService.DeletarPorId(id);
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var resultado = await _tagService.ObterPorId(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return StatusCode(401, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit=50, [FromQuery] int offset=0, [FromQuery] int idTipoTag=0)
    {
        try
        {
            var resultado = await _tagService.ObterTodos(limit, offset, idTipoTag);
            return Ok(resultado);
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
}