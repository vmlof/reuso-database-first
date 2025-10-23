using System;
using System.Collections.Generic;

namespace dbfirst.Data.Models;

public partial class Tag
{
    public int Id { get; set; }

    public int IdTagTipo { get; set; }

    public string Nome { get; set; } = null!;

    public virtual TagTipo IdTagTipoNavigation { get; set; } = null!;
}
