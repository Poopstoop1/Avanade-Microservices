using Domain.Events;
using System.Collections;

namespace Domain.Entities
{
    public abstract class AgreggateRoot : IEntityBase
    {
        public Guid Id { get; protected set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        public IEnumerable<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        protected void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

    }
}
