using dbfirst.Data.Models;
using dbfirst.Data.Models.DTOs;
using dbfirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace dbfirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    public ProdutoController(ProdutoService service)
    {
        _produtoService = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProdutoCreateDto body)
    {
        try
        {
            if (body is null) return BadRequest("Body vazio.");
            if (string.IsNullOrWhiteSpace(body.Nome))
                return BadRequest("Nome é obrigatório.");
            if (body.Preco <= 0)
                return BadRequest("Preco deve ser maior que 0 (em centavos).");

            var resultado = await _produtoService.Criar(body);
            return StatusCode(201, resultado);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(400, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Produto dto)
    {
        try
        {
            var resultado = await _produtoService.Atualizar(id, dto);
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
            await _produtoService.DeletarPorId(id);
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
            var resultado = await _produtoService.ObterPorId(id);
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

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 50, [FromQuery] int offset = 0,
        [FromQuery] string? nome = null, [FromQuery] int? idCategoria = null)
    {
        try
        {
            var resultado = await _produtoService.ObterTodos(limit, offset, nome, idCategoria);
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