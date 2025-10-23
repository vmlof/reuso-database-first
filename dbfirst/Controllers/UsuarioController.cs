using dbfirst.Data.Models;
using dbfirst.Data.Models.DTOs;
using dbfirst.Services;
using dbfirst.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dbfirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] UsuarioRegisterDto dto)
    {
        try
        {
            if (dto is null) return BadRequest("Body vazio.");
            if (string.IsNullOrWhiteSpace(dto.Nome) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Senha))
                return BadRequest("Nome, Email e Senha são obrigatórios.");

            var criado = await _usuarioService.Criar(dto);
            // Remove a senha antes de retornar
            criado.Senha = "";
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(401, ex.Message); // Email em uso
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto login)
    {
        try
        {
            var usuario = await _usuarioService.Login(login.Email, login.Senha);
            // Remove a senha antes de retornar
            usuario.Senha = "";
            return Ok(new
            {
                Usuario = usuario,
                Jwt = Jwt.GenerateToken(usuario)
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var usuario = await _usuarioService.ObterPorId(id);
            usuario.Senha = ""; // Remove a senha
            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario usuario)
    {
        try
        {
            var atualizado = await _usuarioService.Atualizar(id, usuario);
            atualizado.Senha = ""; // Remove a senha
            return Ok(atualizado);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        try
        {
            await _usuarioService.Deletar(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}