
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Data.EntityConfigurations
{
    public class PedidoEntityTypeConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.UsuarioId)
                  .IsRequired();

            builder.Property(p => p.DataCriacao)
                  .IsRequired();

            builder.Property(p => p.Status)
                  .HasConversion<int>()
                  .IsRequired();


            builder.OwnsOne(p => p.ValorTotal, preco =>
            {
                preco.Property(v => v.Valor)
                     .HasColumnName("ValorTotal")
                     .HasPrecision(18, 2)
                     .IsRequired();
            });


            builder.HasMany(p => p.Itens)
                  .WithOne(i => i.Pedido)
                  .HasForeignKey(i => i.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
