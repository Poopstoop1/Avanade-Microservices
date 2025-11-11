using Application.DTOs;
using MediatR;


namespace Application.Query.Handlers
{
    public class GetByStatusPedidoHandler : IRequestHandler<GetByStatusPedido, List<PedidoDTO>>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetByStatusPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<List<PedidoDTO>> Handle(GetByStatusPedido request, CancellationToken cancellationToken)
        {
            var pedidos = await _pedidoRepository.GetByStatusAsync(request.status, cancellationToken);

            return pedidos.Select(p => new PedidoDTO
            {
                UsuarioId = p.UsuarioId,
                DataCriacao = p.DataCriacao,
                ValorTotal = p.ValorTotal,
                Status = p.Status,
                Itens = p.Itens
            }).ToList();
        }

    }
}
