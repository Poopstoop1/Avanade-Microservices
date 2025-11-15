
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class VendasDBContext : DbContext

    {
       
        private readonly IMediator _mediator;
        public VendasDBContext(DbContextOptions<VendasDBContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<AgreggateRoot>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            foreach (var entity in domainEntities)
                entity.Entity.ClearDomainEvents();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);

            return result;
        }


    }
}