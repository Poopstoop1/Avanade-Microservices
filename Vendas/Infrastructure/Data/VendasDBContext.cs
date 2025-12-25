
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
             modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasDBContext).Assembly);
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