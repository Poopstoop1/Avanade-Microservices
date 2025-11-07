using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Vendas.Infrastructure.DB
{
    public class VendasDBContext : DbContext

    {
        
        public VendasDBContext(DbContextOptions<VendasDBContext> options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; } = default!;
        public DbSet<PedidoItem> PedidoItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedidos");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.UsuarioId)
                      .IsRequired();

                entity.Property(p => p.DataCriacao)
                      .IsRequired();

                entity.Property(p => p.Status)
                      .HasConversion<int>() 
                      .IsRequired();

                
                entity.OwnsOne(p => p.ValorTotal, preco =>
                {
                    preco.Property(v => v.Valor)
                         .HasColumnName("ValorTotal")
                         .HasPrecision(18, 2)
                         .IsRequired();
                });

                
                entity.HasMany(p => p.Itens)
                      .WithOne(i => i.Pedido)
                      .HasForeignKey(i => i.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PedidoItem
            modelBuilder.Entity<PedidoItem>(entity =>
            {
                entity.ToTable("PedidoItens");

                entity.HasKey(i => i.Id);

                entity.Property(i => i.ProdutoId)
                      .IsRequired();

                entity.Property(i => i.NomeProduto)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(i => i.Quantidade)
                      .IsRequired();

                entity.Property(i => i.PrecoUnitario)
                      .HasPrecision(18, 2)
                      .IsRequired();
            });
        }


    }
}