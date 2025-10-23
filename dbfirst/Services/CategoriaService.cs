using System.Globalization;
using System.Text;
using dbfirst.Data;
using dbfirst.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class CategoriaService
{
    private readonly ThorDbContext _context;
    public CategoriaService(ThorDbContext context) => _context = context;

    private static string Slugify(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "";
        var nf = s.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(capacity: nf.Length);
        foreach (var ch in nf)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch switch
                {
                    ' ' => '-',
                    _ when char.IsLetterOrDigit(ch) || ch == '-' => ch,
                    _ => '-'
                });
            }
        }

        return sb.ToString().Trim('-');
    }

    public async Task<Categoria> Criar(string nome, string? descricao, bool ativa)
    {
        var slug = Slugify(nome);
        var existente = await _context.Categoria.AnyAsync(c => c.Slug == slug);
        if (existente) throw new InvalidOperationException("Nome/slug já em uso.");

        var c = new Categoria
        {
            Nome = nome,
            Descricao = descricao,
            Ativa = ativa,
            Slug = slug,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };
        _context.Categoria.Add(c);
        await _context.SaveChangesAsync();
        return c;
    }

    public async Task<Categoria> Atualizar(int id, string nome, string? descricao, bool ativa)
    {
        var slug = Slugify(nome);
        var atual = await _context.Categoria.FindAsync(id) ??
                    throw new KeyNotFoundException("Categoria não encontrada.");
        var conflito = await _context.Categoria.FirstOrDefaultAsync(c => c.Slug == slug);
        if (conflito != null && conflito.Id != id) throw new InvalidOperationException("Nome/slug já em uso.");

        atual.Nome = nome;
        atual.Slug = slug;
        atual.Descricao = descricao;
        atual.Ativa = ativa;
        atual.AtualizadoEm = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return atual;
    }

    public Task<Categoria?> ObterPorSlug(string slug) => _context.Categoria.FirstOrDefaultAsync(c => c.Slug == slug);
    public Task<Categoria?> ObterPorId(int id) => _context.Categoria.FindAsync(id).AsTask();

    public async Task<IEnumerable<Categoria>> Listar(int limit, int offset, string? nome = null)
    {
        var query = _context.Categoria.AsQueryable();
        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(c => c.Nome.ToLower().Contains(nome.ToLower()));
        }

        return await query
            .OrderBy(c => c.Nome)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task Deletar(int id)
    {
        var c = await _context.Categoria.FindAsync(id);
        if (c != null)
        {
            _context.Categoria.Remove(c);
            await _context.SaveChangesAsync();
        }
    }
}