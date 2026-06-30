using Microsoft.EntityFrameworkCore;
using gstok_api.Models;
using gstok_api.Enums;

namespace gstok_api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Estoque> Estoques { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Estoque>()
            .Property(e => e.TpTamanho)
            .HasConversion<string>();
    }
}
