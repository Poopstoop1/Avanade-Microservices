using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Event.Producers.Handlers
{
    public class PedidoCriadoEventHandler : INotificationHandler<PedidoCriadoEvent>
    {
        private readonly IMessageBusClient _bus;

        public PedidoCriadoEventHandler(IMessageBusClient bus)
        {
            _bus = bus;
        }

        public async Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken)
        {
            await _bus.Publish(notification, "pedido-criado", "pedido.exchange", cancellationToken);
        }

    }
}
