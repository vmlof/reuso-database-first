using dbfirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace dbfirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrinhoController : ControllerBase
{
    private readonly CarrinhoService _carrinhoService;

    public CarrinhoController(CarrinhoService carrinhoService)
    {
        _carrinhoService = carrinhoService;
    }

    [HttpPut("{idUsuario}/{idProduto}")]
    public async Task<IActionResult> Atualizar(int idUsuario, int idProduto, [FromQuery] int quantidade)
    {
        try
        {
            var resultado = await _carrinhoService.AtualizarCarrinho(idUsuario, idProduto, quantidade);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{idUsuario}")]
    public async Task<IActionResult> Get(int idUsuario)
    {
        try
        {
            var resultado = await _carrinhoService.ObterCarrinho(idUsuario);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{idUsuario}")]
    public async Task<IActionResult> Delete(int idUsuario)
    {
        try
        {
            await _carrinhoService.RemoverCarrinho(idUsuario);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}