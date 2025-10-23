using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class Endereco
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Rua { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string? Complemento { get; set; }

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Cep { get; set; } = null!;

    public bool IsPrincipal { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
