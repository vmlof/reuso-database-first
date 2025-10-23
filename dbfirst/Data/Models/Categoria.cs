using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Descricao { get; set; }

    public bool? Ativa { get; set; }

    public DateTime? CriadoEm { get; set; }

    public DateTime? AtualizadoEm { get; set; }

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
