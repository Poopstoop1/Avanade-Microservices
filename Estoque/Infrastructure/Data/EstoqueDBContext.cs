
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Infrastructure.Data
{
    public class EstoqueDBContext : DbContext
    {
        private readonly IMediator _mediator;
        public EstoqueDBContext(DbContextOptions<EstoqueDBContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
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


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<AggregateRoot>()
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