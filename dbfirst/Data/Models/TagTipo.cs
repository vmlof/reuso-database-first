using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class TagTipo
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
