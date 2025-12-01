using Application.DTOs;
using Application.DTOs.ViewModels;
using MediatR;


namespace Application.Queries.Handlers
{
    public class GetByStatusPedidoHandler : IRequestHandler<GetByStatusPedido, List<PedidoViewDTO>>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetByStatusPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<List<PedidoViewDTO>> Handle(GetByStatusPedido request, CancellationToken cancellationToken)
        {
            var pedidos = await _pedidoRepository.GetByStatusAsync(request.Status, cancellationToken);

            return 
            [
                ..pedidos.Select(p => new PedidoViewDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    DataCriacao = p.DataCriacao,
                    ValorTotal = p.ValorTotal,
                    Status = p.Status,
                    Itens = 
                    [
                        ..p.Itens.Select(i => new PedidoItemDTO
                        {
                            ProdutoId = i.ProdutoId,
                            Quantidade = i.Quantidade,
                            PrecoUnitario = i.PrecoUnitario
                        })
                    ]

                })
            ];
        }

    }
}
