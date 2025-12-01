using MediatR;

namespace Application.Command.Handlers
{
    public class DeletePedidoHandler(IPedidoRepository pedidoRepository) : IRequestHandler<DeletePedido, Unit>
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

     

        public async Task<Unit> Handle(DeletePedido request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pedido com id {request.Id} não encontrado");
            
            await _pedidoRepository.DeleteAsync(pedido, cancellationToken);
            return Unit.Value;
        }
    }
}
