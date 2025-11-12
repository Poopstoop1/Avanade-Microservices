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
            var pedidos = await _pedidoRepository.GetByStatusAsync(request.Status, cancellationToken);

            return pedidos.Select(p => new PedidoDTO
            {
                UsuarioId = p.UsuarioId,
                DataCriacao = p.DataCriacao,
                ValorTotal = p.ValorTotal,
                Status = p.Status,
                Itens = p.Itens.Select(i => new PedidoItemDTO
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()

            }).ToList();
        }

    }
}
