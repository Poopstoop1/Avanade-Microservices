using Application.DTOs;
using MediatR;


namespace Application.Query.Handlers
{
    public class GetByIdPedidoHandler : IRequestHandler<GetByIdPedido, PedidoDTO>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetByIdPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDTO> Handle(GetByIdPedido request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (pedido == null)
            {
                throw new KeyNotFoundException($"Pedido com Id {request.Id} não encontrado."); 
            }
            return new PedidoDTO
            {
                UsuarioId = pedido.UsuarioId,
                DataCriacao = pedido.DataCriacao,
                ValorTotal = pedido.ValorTotal,
                Status = pedido.Status,
                Itens = pedido.Itens
            };
        }

    }
}
