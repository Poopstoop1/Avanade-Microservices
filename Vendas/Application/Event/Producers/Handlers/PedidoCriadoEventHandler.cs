using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Event.Producers.Handlers
{
    public class PedidoCriadoEventHandler(IMessageBusClient bus) : INotificationHandler<PedidoCriadoEvent>
    {
        private readonly IMessageBusClient _bus = bus;

        public async Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken)
        {
            await _bus.Publish(notification, "pedido-criado", "pedido.exchange", cancellationToken);
        }

    }
}
