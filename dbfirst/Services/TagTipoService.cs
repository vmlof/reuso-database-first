using dbfirst.Data;
using dbfirst.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class TagTipoService
{
    private readonly ThorDbContext _context;

    public TagTipoService(ThorDbContext context)
    {
        _context = context;
    }

    public async Task<TagTipo> Atualiza(int id, TagTipo tagTipo)
    {
        var existente = await _context.TagTipos.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("O tipo de tag não foi encontrado");

        var conflito = await _context.TagTipos.FirstOrDefaultAsync(t => t.Nome == tagTipo.Nome);
        if (conflito != null && conflito.Id != id)
        {
            throw new InvalidOperationException("Este nome de tipo de tag ja esta sendo utilizado");
        }

        existente.Nome = tagTipo.Nome;
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<TagTipo> Criar(TagTipo tagTipo)
    {
        var existente = await _context.TagTipos.AnyAsync(t => t.Nome == tagTipo.Nome);
        if (existente) throw new InvalidOperationException("O nome já está sendo utilizado");

        _context.TagTipos.Add(tagTipo);
        await _context.SaveChangesAsync();
        return tagTipo;
    }

    public async Task DeletarPorId(int id)
    {
        var existente = await _context.TagTipos.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tipo de tag não encontrado para exclusão");

        _context.TagTipos.Remove(existente);
        await _context.SaveChangesAsync();
    }

    public async Task<TagTipo?> ObterPorId(int id)
    {
        return await _context.TagTipos.FindAsync(id);
    }

    public async Task<IEnumerable<TagTipo>> ObterTodos(int limit, int offset)
    {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");
        return await _context.TagTipos
            .OrderBy(t => t.Nome)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}