using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using dbfirst.Data.Models;

namespace dbfirst.Data;

public partial class ThorDbContext : DbContext
{
    public ThorDbContext()
    {
    }

    public ThorDbContext(DbContextOptions<ThorDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrinho> Carrinhos { get; set; }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Endereco> Enderecos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagTipo> TagTipos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;DataBase=dbfirst;Uid=postgres;Pwd=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrinho>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdProduto }).HasName("carrinho_pkey");

            entity.ToTable("carrinho");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdProduto).HasColumnName("id_produto");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdProdutoNavigation).WithMany(p => p.Carrinhos)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrinho_id_produto_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Carrinhos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrinho_id_usuario_fkey");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categoria_pkey");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.Slug, "categoria_slug_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativa)
                .HasDefaultValue(true)
                .HasColumnName("ativa");
            entity.Property(e => e.AtualizadoEm)
                .HasDefaultValueSql("now()")
                .HasColumnName("atualizado_em");
            entity.Property(e => e.CriadoEm)
                .HasDefaultValueSql("now()")
                .HasColumnName("criado_em");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .HasColumnName("slug");
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("endereco_pkey");

            entity.ToTable("endereco");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(100)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(100)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(100)
                .HasColumnName("complemento");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IsPrincipal)
                .HasDefaultValue(false)
                .HasColumnName("is_principal");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(200)
                .HasColumnName("rua");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Enderecos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("endereco_id_usuario_fkey");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("produto_pkey");

            entity.ToTable("produto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AtualizadoEm)
                .HasDefaultValueSql("now()")
                .HasColumnName("atualizado_em");
            entity.Property(e => e.CriadoEm)
                .HasDefaultValueSql("now()")
                .HasColumnName("criado_em");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdTagTipo).HasColumnName("id_tag_tipo");
            entity.Property(e => e.Imagem).HasColumnName("imagem");
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .HasColumnName("nome");
            entity.Property(e => e.Preco).HasColumnName("preco");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("produto_id_categoria_fkey");

            entity.HasOne(d => d.IdTagTipoNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.IdTagTipo)
                .HasConstraintName("produto_id_tag_tipo_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pkey");

            entity.ToTable("tag");

            entity.HasIndex(e => new { e.IdTagTipo, e.Nome }, "tag_id_tag_tipo_nome_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTagTipo).HasColumnName("id_tag_tipo");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdTagTipoNavigation).WithMany(p => p.Tags)
                .HasForeignKey(d => d.IdTagTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tag_id_tag_tipo_fkey");
        });

        modelBuilder.Entity<TagTipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_tipo_pkey");

            entity.ToTable("tag_tipo");

            entity.HasIndex(e => e.Nome, "tag_tipo_nome_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Cpf, "usuario_cpf_key").IsUnique();

            entity.HasIndex(e => e.Email, "usuario_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Senha).HasColumnName("senha");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .HasDefaultValueSql("'usuario'::character varying")
                .HasColumnName("tipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
