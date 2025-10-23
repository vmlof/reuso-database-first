﻿namespace dbfirst.Data.Models.DTOs;

public sealed class UsuarioRegisterDto
{
    public string Cpf { get; set; } = default!;
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
}

public sealed class UsuarioLoginDto
{
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
}