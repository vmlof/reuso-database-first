using dbfirst.Data;
using dbfirst.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace dbfirst.Services;

public class CarrinhoService
{
    private readonly ThorDbContext _context;

    public CarrinhoService(ThorDbContext context)
    {
        _context = context;
    }

    public async Task<object> AtualizarCarrinho(int idUsuario, int idProduto, int quantidade)
    {
        // Chave primária composta (IdUsuario, IdProduto)
        var existente = await _context.Carrinhos.FindAsync(idUsuario, idProduto);

        if (existente == null && quantidade > 0)
        {
            _context.Carrinhos.Add(new Carrinho()
            {
                IdUsuario = idUsuario,
                IdProduto = idProduto,
                Quantidade = quantidade
            });
        }
        else if (existente != null && quantidade <= 0)
        {
            _context.Carrinhos.Remove(existente);
        }
        else if (existente != null && quantidade > 0)
        {
            existente.Quantidade = quantidade;
        }
        else if (existente == null && quantidade <= 0)
        {
            throw new InvalidOperationException("O item precisa ter uma quantidade de pelo menos um para ser incluido");
        }

        await _context.SaveChangesAsync();
        return await ObterCarrinho(idUsuario);
    }

    public async Task<object> ObterCarrinho(int idUsuario)
    {
        var itens = await _context.Carrinhos
            .Where(c => c.IdUsuario == idUsuario)
            .Include(c => c.IdProdutoNavigation) // Junta com a tabela Produto
            .Select(c => new
            {
                IdProduto = c.IdProduto,
                Produto = c.IdProdutoNavigation.Nome,
                Quantidade = c.Quantidade,
                PrecoUnitario = c.IdProdutoNavigation.Preco,
                PrecoTotal = c.IdProdutoNavigation.Preco * c.Quantidade
            })
            .ToListAsync();

        var total = itens.Sum(i => i.PrecoTotal);

        return new { Total = total, Produtos = itens };
    }

    public async Task RemoverCarrinho(int idUsuario)
    {
        // Remove todos os itens do carrinho para este usuário
        await _context.Carrinhos
            .Where(c => c.IdUsuario == idUsuario)
            .ExecuteDeleteAsync();
    }
}