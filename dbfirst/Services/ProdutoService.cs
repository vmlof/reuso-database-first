using dbfirst.Data;
using dbfirst.Data.Models;
using dbfirst.Data.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class ProdutoService
{
    private readonly ThorDbContext _context;

    public ProdutoService(ThorDbContext context)
    {
        _context = context;
    }

    public async Task<Produto> Criar(ProdutoCreateDto dto)
    {
        if (dto.IdTagTipo != null)
        {
            var tagTipo = await _context.TagTipos.FindAsync(dto.IdTagTipo.Value);
            if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");
        }
        
        if (dto.IdCategoria != null)
        {
            var categoria = await _context.Categoria.FindAsync(dto.IdCategoria.Value);
            if (categoria == null) throw new InvalidOperationException("A categoria solicitada não existe");
        }

        var produto = new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Imagem = dto.Imagem,
            Preco = dto.Preco,
            IdTagTipo = dto.IdTagTipo,
            IdCategoria = dto.IdCategoria,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto> Atualizar(int id, Produto produtoAtualizado)
    {
        var existente = await _context.Produtos.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Produto não encontrado para atualização");

        if (produtoAtualizado.IdTagTipo != null)
        {
            var tagTipo = await _context.TagTipos.FindAsync(produtoAtualizado.IdTagTipo.Value);
            if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");
        }

        existente.Nome = produtoAtualizado.Nome;
        existente.Descricao = produtoAtualizado.Descricao;
        existente.Imagem = produtoAtualizado.Imagem;
        existente.Preco = produtoAtualizado.Preco;
        existente.IdTagTipo = produtoAtualizado.IdTagTipo;
        existente.IdCategoria = produtoAtualizado.IdCategoria;
        existente.AtualizadoEm = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task DeletarPorId(int id)
    {
        var existente = await _context.Produtos.FindAsync(id);
        if (existente == null) throw new KeyNotFoundException("Produto não encontrado para exclusão");

        _context.Produtos.Remove(existente);
        await _context.SaveChangesAsync();
    }

    public async Task<Produto> ObterPorId(int id)
    {
        var produto = await _context.Produtos
            .Include(p => p.IdCategoriaNavigation)
            .Include(p => p.IdTagTipoNavigation)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (produto == null) throw new KeyNotFoundException("Produto não encontrado");
        return produto;
    }

    public async Task<IEnumerable<Produto>> ObterTodos(int limit, int offset, string? nome = null, int? idCategoria = null)
    {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        var query = _context.Produtos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(p => p.Nome.ToLower().Contains(nome.ToLower()));
        }

        if (idCategoria.HasValue)
        {
            query = query.Where(p => p.IdCategoria == idCategoria);
        }

        return await query
            .OrderByDescending(p => p.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}