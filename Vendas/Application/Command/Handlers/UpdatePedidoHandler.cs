using Domain.Enums;
using MediatR;


namespace Application.Command.Handlers
{
    public class UpdatePedidoHandler : IRequestHandler<UpdatePedido, Unit>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public UpdatePedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
 
        }

        public async Task<Unit> Handle(UpdatePedido request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pedido com id {request.Id} não encontrado");
           

            pedido.Status = request.Status;


            if (request.Status == PedidoStatus.Confirmado)
            {
              
                pedido.PedidoConfirmado();
               // await _messageBusClient.Publish(evento, "pedido.confirmado", "pedido_exchange", cancellationToken);
            }
            await _pedidoRepository.UpdateAsync(pedido, cancellationToken);

            return Unit.Value;
        }
    }
}
