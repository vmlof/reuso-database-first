using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public string? Imagem { get; set; }

    public int Preco { get; set; }

    public DateTime? CriadoEm { get; set; }

    public DateTime? AtualizadoEm { get; set; }

    public int? IdTagTipo { get; set; }

    public int? IdCategoria { get; set; }

    public virtual ICollection<Carrinho> Carrinhos { get; set; } = new List<Carrinho>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual TagTipo? IdTagTipoNavigation { get; set; }
}
