using Microsoft.EntityFrameworkCore;
using gstok_api.Enums;
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
    public DbSet<ContaClienteModel> ContasCliente { get; set; }
    public DbSet<VendaModel> Vendas { get; set; }
    public DbSet<VendaItemModel> ItensVenda { get; set; }
    public DbSet<FornecedorModel> Fornecedores { get; set; }
    public DbSet<ColecaoModel> Colecoes { get; set; }
    public DbSet<CorProdutoModel> CoresProduto { get; set; }
    public DbSet<CompraModel> Compras { get; set; }
    public DbSet<CompraItemModel> ItensCompra { get; set; }
    public DbSet<PromocaoModel> Promocoes { get; set; }
    public DbSet<DevolucaoModel> Devolucoes { get; set; }
    public DbSet<DevolucaoItemModel> ItensDevolucao { get; set; }
    public DbSet<TrocaModel> Trocas { get; set; }
    public DbSet<TrocaItemSaidaModel> ItensTrocaSaida { get; set; }
    public DbSet<TrocaItemEntradaModel> ItensTrocaEntrada { get; set; }
    public DbSet<PromocaoProdutoModel> ProdutosPromocao { get; set; }
    public DbSet<EnderecoModel> Enderecos { get; set; }
    public DbSet<CarrinhoModel> Carrinhos { get; set; }
    public DbSet<CarrinhoItemModel> ItensCarrinho { get; set; }
    public DbSet<SessaoClienteModel> SessoesCliente { get; set; }

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

        modelBuilder.Entity<ProdutoModel>()
            .Property(p => p.TpGenero)
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
            .HasIndex(s => s.CdToken)
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
            .OnDelete(DeleteBehavior.Restrict);

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

        modelBuilder.Entity<ContaClienteModel>()
            .HasOne(cc => cc.Cliente)
            .WithOne(c => c.ContaCliente)
            .HasForeignKey<ContaClienteModel>(cc => cc.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ContaClienteModel>()
            .HasIndex(cc => cc.ClienteId)
            .IsUnique();

        modelBuilder.Entity<ContaClienteModel>()
            .HasIndex(cc => cc.NmEmail)
            .IsUnique();

        modelBuilder.Entity<VendaModel>()
            .Property(p => p.StVenda)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<VendaModel>()
            .Property(p => p.StPagamento)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<VendaModel>()
            .Property(p => p.TpPagamento)
            .HasConversion<string>()
            .HasMaxLength(10);

        modelBuilder.Entity<VendaModel>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Compras)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendaItemModel>()
            .HasOne(i => i.Venda)
            .WithMany(p => p.Itens)
            .HasForeignKey(i => i.VendaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VendaItemModel>()
            .HasOne(i => i.Estoque)
            .WithMany()
            .HasForeignKey(i => i.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FornecedorModel>()
            .HasIndex(f => f.CdCnpj)
            .IsUnique();

        modelBuilder.Entity<ColecaoModel>()
            .HasOne(c => c.Fornecedor)
            .WithMany(f => f.Colecoes)
            .HasForeignKey(c => c.FornecedorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ColecaoModel>()
            .HasIndex(c => new { c.FornecedorId, c.NmColecao })
            .IsUnique();

        modelBuilder.Entity<ProdutoModel>()
            .HasOne(p => p.Colecao)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.ColecaoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CorProdutoModel>()
            .HasOne(c => c.Produto)
            .WithMany(p => p.CoresProduto)
            .HasForeignKey(c => c.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CorProdutoModel>()
            .HasIndex(c => new { c.ProdutoId, c.NmCor })
            .IsUnique();

        modelBuilder.Entity<EstoqueModel>()
            .HasOne(e => e.CorProduto)
            .WithMany()
            .HasForeignKey(e => e.CorProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EstoqueModel>()
            .HasIndex(e => new { e.CorProdutoId, e.TpTamanho })
            .IsUnique();

        modelBuilder.Entity<CompraModel>()
            .Property(c => c.StCompra)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<CompraModel>()
            .HasOne(c => c.Fornecedor)
            .WithMany(f => f.Compras)
            .HasForeignKey(c => c.FornecedorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompraItemModel>()
            .Property(i => i.TpTamanho)
            .HasConversion<string>();

        modelBuilder.Entity<CompraItemModel>()
            .HasOne(i => i.Compra)
            .WithMany(c => c.Itens)
            .HasForeignKey(i => i.CompraId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompraItemModel>()
            .HasOne(i => i.CorProduto)
            .WithMany()
            .HasForeignKey(i => i.CorProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompraItemModel>()
            .HasOne(i => i.Estoque)
            .WithMany()
            .HasForeignKey(i => i.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PromocaoProdutoModel>()
            .HasOne(pp => pp.Promocao)
            .WithMany(p => p.Produtos)
            .HasForeignKey(pp => pp.PromocaoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PromocaoProdutoModel>()
            .HasOne(pp => pp.Produto)
            .WithMany(p => p.Promocoes)
            .HasForeignKey(pp => pp.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PromocaoProdutoModel>()
            .HasIndex(pp => new { pp.PromocaoId, pp.ProdutoId })
            .IsUnique();

        modelBuilder.Entity<DevolucaoModel>()
            .Property(d => d.StDevolucao)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<DevolucaoModel>()
            .Property(d => d.TpReembolso)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<DevolucaoModel>()
            .HasOne(d => d.Venda)
            .WithMany()
            .HasForeignKey(d => d.VendaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DevolucaoItemModel>()
            .HasOne(i => i.Devolucao)
            .WithMany(d => d.Itens)
            .HasForeignKey(i => i.DevolucaoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DevolucaoItemModel>()
            .HasOne(i => i.VendaItem)
            .WithMany()
            .HasForeignKey(i => i.VendaItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrocaModel>()
            .Property(t => t.StTroca)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<TrocaModel>()
            .Property(t => t.TpPagamento)
            .HasConversion<string>()
            .HasMaxLength(10);

        modelBuilder.Entity<TrocaModel>()
            .Property(t => t.TpReembolso)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<TrocaModel>()
            .HasOne(t => t.Venda)
            .WithMany()
            .HasForeignKey(t => t.VendaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrocaItemSaidaModel>()
            .HasOne(i => i.Troca)
            .WithMany(t => t.ItensSaida)
            .HasForeignKey(i => i.TrocaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TrocaItemSaidaModel>()
            .HasOne(i => i.VendaItem)
            .WithMany()
            .HasForeignKey(i => i.VendaItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrocaItemEntradaModel>()
            .HasOne(i => i.Troca)
            .WithMany(t => t.ItensEntrada)
            .HasForeignKey(i => i.TrocaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TrocaItemEntradaModel>()
            .HasOne(i => i.Estoque)
            .WithMany()
            .HasForeignKey(i => i.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EnderecoModel>()
            .HasOne(e => e.Cliente)
            .WithMany(c => c.Enderecos)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EnderecoModel>()
            .HasIndex(e => e.ClienteId)
            .HasFilter("fl_principal = true")
            .IsUnique();

        modelBuilder.Entity<CarrinhoModel>()
            .HasOne(c => c.Cliente)
            .WithOne(cl => cl.Carrinho)
            .HasForeignKey<CarrinhoModel>(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CarrinhoModel>()
            .HasIndex(c => c.ClienteId)
            .IsUnique();

        modelBuilder.Entity<CarrinhoItemModel>()
            .HasOne(i => i.Carrinho)
            .WithMany(c => c.Itens)
            .HasForeignKey(i => i.CarrinhoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CarrinhoItemModel>()
            .HasOne(i => i.Estoque)
            .WithMany()
            .HasForeignKey(i => i.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CarrinhoItemModel>()
            .HasIndex(i => new { i.CarrinhoId, i.EstoqueId })
            .IsUnique();

        modelBuilder.Entity<SessaoClienteModel>()
            .HasIndex(s => s.CdToken)
            .IsUnique();

        modelBuilder.Entity<SessaoClienteModel>()
            .HasOne(s => s.Cliente)
            .WithMany()
            .HasForeignKey(s => s.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VendaModel>()
            .Property(p => p.TpOrigem)
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasDefaultValue(TipoOrigemVenda.Loja);

        modelBuilder.Entity<VendaModel>()
            .HasOne(v => v.EnderecoEntrega)
            .WithMany()
            .HasForeignKey(v => v.EnderecoEntregaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
