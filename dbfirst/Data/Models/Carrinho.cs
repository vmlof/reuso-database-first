using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class Carrinho
{
    public int IdUsuario { get; set; }

    public int IdProduto { get; set; }

    public int Quantidade { get; set; }

    public virtual Produto IdProdutoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
