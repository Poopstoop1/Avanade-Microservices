using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Event.Producers.Handlers
{
    public class PedidoConfirmadoEventHandler(IMessageBusClient bus) : INotificationHandler<PedidoConfirmadoEvent>
    {
        private readonly IMessageBusClient _bus = bus;

        public async Task Handle(PedidoConfirmadoEvent notification, CancellationToken cancellationToken)
        {
            await _bus.Publish(notification,"pedido.confirmado","pedido_exchange",cancellationToken);
        }
    }
}
