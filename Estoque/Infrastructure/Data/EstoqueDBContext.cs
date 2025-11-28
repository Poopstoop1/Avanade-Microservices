
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class EstoqueDBContext : DbContext
    {
        public EstoqueDBContext(DbContextOptions<EstoqueDBContext> options) : base(options)
        {   
        }

        public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Descricao).HasMaxLength(500);
            entity.OwnsOne(p => p.Preco, preco =>
                {
                    preco.Property(p => p.Valor)
                    .HasColumnName("Preco")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                });
            entity.Property(p => p.Quantidade).HasColumnName("Quantidade").IsRequired();
            entity.Property(p => p.QuantidadeReservada).HasColumnName("QuantidadeReservada");
        });
    }

  }
}