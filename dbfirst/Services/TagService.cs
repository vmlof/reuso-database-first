using dbfirst.Data;
using dbfirst.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class TagService
{
    private readonly ThorDbContext _context;

    public TagService(ThorDbContext context)
    {
        _context = context;
    }

    public async Task<Tag> Atualizar(int id, Tag dto)
    {
        var existente = await _context.Tags.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para atualização");

        var tagTipo = await _context.TagTipos.FindAsync(dto.IdTagTipo);
        if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");

        existente.Nome = dto.Nome;
        existente.IdTagTipo = dto.IdTagTipo;
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<Tag> Criar(Tag dto)
    {
        var existente = await _context.Tags.AnyAsync(t => t.Nome == dto.Nome && t.IdTagTipo == dto.IdTagTipo);
        if (existente) throw new InvalidOperationException("O nome já está sendo utilizado neste tipo de tag");

        var tagTipo = await _context.TagTipos.FindAsync(dto.IdTagTipo);
        if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");

        _context.Tags.Add(dto);
        await _context.SaveChangesAsync();
        return dto;
    }

    public async Task DeletarPorId(int id)
    {
        var existente = await _context.Tags.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para exclusão");

        _context.Tags.Remove(existente);
        await _context.SaveChangesAsync();
    }

    public async Task<Tag> ObterPorId(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) throw new KeyNotFoundException("Tag não encontrada");
        return tag;
    }

    public async Task<IEnumerable<Tag>> ObterTodos(int limit, int offset, int idTipoTag)
    {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        return await _context.Tags
            .Where(t => t.IdTagTipo == idTipoTag)
            .OrderBy(t => t.Nome)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}