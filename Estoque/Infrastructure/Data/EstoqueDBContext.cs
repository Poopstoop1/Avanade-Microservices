using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.DB
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
        entity.Property(p => p.Preco).HasColumnType("decimal(18,2)");
        });
    }

  }
}