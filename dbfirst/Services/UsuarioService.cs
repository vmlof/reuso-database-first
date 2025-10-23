using dbfirst.Data;
using dbfirst.Data.Models;
using dbfirst.Data.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class UsuarioService
{
    private readonly ThorDbContext _context;

    public UsuarioService(ThorDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> Criar(UsuarioRegisterDto dto)
    {
        var jaExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (jaExiste) throw new InvalidOperationException("Email ja esta sendo utilizado");

        var usuario = new Usuario
        {
            Cpf = dto.Cpf,
            Nome = dto.Nome,
            Email = dto.Email,
            // Importante: Armazenar senhas com hash
            Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Tipo = "usuario"
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> Login(string email, string senha)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null) throw new UnauthorizedAccessException("Email ou senha inválidos");

        // Verificar o hash da senha
        if (!BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
        {
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        }

        return usuario;
    }

    public async Task<Usuario> ObterPorId(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) throw new KeyNotFoundException("Usuário não encontrado");
        return usuario;
    }

    public async Task<Usuario> Atualizar(int id, Usuario usuarioAtualizado)
    {
        var existente = await _context.Usuarios.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Usuário não encontrado");

        existente.Nome = usuarioAtualizado.Nome;
        existente.Email = usuarioAtualizado.Email;
        existente.Cpf = usuarioAtualizado.Cpf;

        // Lógica para atualização de senha (se fornecida e for diferente)
        if (!string.IsNullOrWhiteSpace(usuarioAtualizado.Senha) &&
            !BCrypt.Net.BCrypt.Verify(usuarioAtualizado.Senha, existente.Senha))
        {
            existente.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizado.Senha);
        }

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task Deletar(int id)
    {
        var existente = await _context.Usuarios.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Usuário não encontrado");

        _context.Usuarios.Remove(existente);
        await _context.SaveChangesAsync();
    }
}