using Microsoft.EntityFrameworkCore;
using gstok_api.Models;

namespace gstok_api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PessoaModel> Pessoas { get; set; }
    public DbSet<ProdutoModel> Produtos { get; set; }
    public DbSet<EstoqueModel> Estoques { get; set; }
    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<SessaoModel> Sessoes { get; set; }
    public DbSet<ImagemProdutoModel> ImagensProduto { get; set; }
    public DbSet<TipoProdutoModel> TiposProduto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EstoqueModel>()
            .Property(e => e.TpTamanho)
            .HasConversion<string>();

        modelBuilder.Entity<UsuarioModel>()
            .HasIndex(u => u.NmEmail)
            .IsUnique();

        modelBuilder.Entity<UsuarioModel>()
            .HasOne(u => u.Pessoa)
            .WithOne()
            .HasForeignKey<UsuarioModel>(u => u.PessoaId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UsuarioModel>()
            .HasIndex(u => u.PessoaId)
            .IsUnique()
            .HasFilter("pessoa_id IS NOT NULL");

        modelBuilder.Entity<SessaoModel>()
            .HasIndex(s => s.CdRefreshToken)
            .IsUnique();

        modelBuilder.Entity<SessaoModel>()
            .HasOne(s => s.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ImagemProdutoModel>()
            .HasOne(i => i.Produto)
            .WithMany(p => p.Imagens)
            .HasForeignKey(i => i.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TipoProdutoModel>()
            .HasIndex(t => t.NmTipo)
            .IsUnique();

        modelBuilder.Entity<ProdutoModel>()
            .HasOne(p => p.TipoProduto)
            .WithMany(t => t.Produtos)
            .HasForeignKey(p => p.TipoProdutoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
