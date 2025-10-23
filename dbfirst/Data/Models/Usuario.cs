using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Carrinho> Carrinhos { get; set; } = new List<Carrinho>();

    public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
}
