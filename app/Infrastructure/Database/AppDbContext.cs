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
    public DbSet<FotoPessoaModel> FotosPessoa { get; set; }
    public DbSet<ClienteModel> Clientes { get; set; }
    public DbSet<PedidoModel> Pedidos { get; set; }
    public DbSet<ItemPedidoModel> ItensPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EstoqueModel>()
            .Property(e => e.TpTamanho)
            .HasConversion<string>();

        modelBuilder.Entity<PessoaModel>()
            .Property(p => p.TpPessoa)
            .HasConversion<string>()
            .HasMaxLength(1);

        modelBuilder.Entity<ProdutoModel>()
            .Property(p => p.TpEstacao)
            .HasConversion<string>()
            .HasMaxLength(10);

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

        modelBuilder.Entity<PessoaModel>()
            .HasIndex(p => p.CdInscricaoNacional)
            .IsUnique();

        modelBuilder.Entity<FotoPessoaModel>()
            .HasOne(f => f.Pessoa)
            .WithOne(p => p.Foto)
            .HasForeignKey<FotoPessoaModel>(f => f.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClienteModel>()
            .HasOne(c => c.Pessoa)
            .WithOne(p => p.Cliente)
            .HasForeignKey<ClienteModel>(c => c.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClienteModel>()
            .HasIndex(c => c.PessoaId)
            .IsUnique();

        modelBuilder.Entity<PedidoModel>()
            .Property(p => p.StPedido)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<PedidoModel>()
            .Property(p => p.StPagamento)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<PedidoModel>()
            .Property(p => p.TpPagamento)
            .HasConversion<string>()
            .HasMaxLength(10);

        modelBuilder.Entity<PedidoModel>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItemPedidoModel>()
            .HasOne(i => i.Pedido)
            .WithMany(p => p.Itens)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemPedidoModel>()
            .HasOne(i => i.Estoque)
            .WithMany()
            .HasForeignKey(i => i.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
