using dbfirst.Data;
using dbfirst.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class EnderecoService
{
    private readonly ThorDbContext _context;

    public EnderecoService(ThorDbContext context)
    {
        _context = context;
    }

    public async Task<Endereco> CriarAsync(Endereco endereco)
    {
        var usuario = await _context.Usuarios.FindAsync(endereco.IdUsuario);
        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        if (endereco.IsPrincipal)
        {
            // Define todos os outros endereços deste usuário como NÃO principais
            await _context.Enderecos
                .Where(e => e.IdUsuario == endereco.IdUsuario && e.IsPrincipal)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsPrincipal, false));
        }

        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();
        return endereco;
    }

    public async Task<IEnumerable<Endereco>> ObterPorUsuarioIdAsync(int idUsuario)
    {
        return await _context.Enderecos
            .Where(e => e.IdUsuario == idUsuario)
            .ToListAsync();
    }

    public async Task<Endereco> AtualizarAsync(int id, Endereco endereco)
    {
        var existente = await _context.Enderecos.FindAsync(id);
        if (existente == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        // Garante que o usuário do endereço não seja alterado
        endereco.IdUsuario = existente.IdUsuario;

        if (endereco.IsPrincipal)
        {
            // Define todos os outros endereços deste usuário como NÃO principais
            await _context.Enderecos
                .Where(e => e.IdUsuario == existente.IdUsuario && e.Id != id && e.IsPrincipal)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsPrincipal, false));
        }

        // Atualiza os campos
        _context.Entry(existente).CurrentValues.SetValues(endereco);
        existente.IsPrincipal = endereco.IsPrincipal; // Garante que IsPrincipal seja atualizado

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task DeletarAsync(int id)
    {
        var existente = await _context.Enderecos.FindAsync(id);
        if (existente == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        _context.Enderecos.Remove(existente);
        await _context.SaveChangesAsync();
    }
}