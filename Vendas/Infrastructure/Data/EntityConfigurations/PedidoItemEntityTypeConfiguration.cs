

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Data.EntityConfigurations
{
    public class PedidoItemEntityTypeConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProdutoId)
                  .IsRequired();

            builder.Property(i => i.NomeProduto)
                  .HasMaxLength(200)
                  .IsRequired();

            builder.Property(i => i.Quantidade)
                  .IsRequired();

            builder.Property(i => i.PrecoUnitario)
                  .HasPrecision(18, 2)
                  .IsRequired();
        }
    }
}
