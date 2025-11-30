using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Event.Consumers.Handlers
{
    public class EstoqueRejeitadoConsumerHandler : IRequestHandler<EstoqueRejeitadoEvent>
    {

        private readonly IPedidoRepository _pedidoRepository;

        private readonly ILogger<EstoqueRejeitadoConsumerHandler> _logger;

        public EstoqueRejeitadoConsumerHandler(IPedidoRepository pedidoRepository, ILogger<EstoqueRejeitadoConsumerHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }

        public async Task Handle(EstoqueRejeitadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Pedido {PedidoId} foi rejeitado pelo estoque. Motivo: {Motivo}",
            notification.PedidoId,
            notification.Motivo);

            var pedido = await _pedidoRepository.GetByIdAsync(notification.PedidoId, cancellationToken);
            _ = pedido ?? throw new KeyNotFoundException($"Produto {notification.PedidoId} não encontrado.");

            pedido.PedidoCancelado();

            await _pedidoRepository.UpdateAsync(pedido, cancellationToken);
        }

    }
}
