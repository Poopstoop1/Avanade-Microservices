using Application.DTOs;
using MediatR;

namespace Application.Query.Handlers
{
    public class GetAllPedidoHandler : IRequestHandler<GetByAllPedido, List<PedidoDTO>>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetAllPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }


        public async Task<List<PedidoDTO>> Handle(GetByAllPedido request, CancellationToken cancellationToken)
        {
            var produtos = await _pedidoRepository.GetAllAsync(cancellationToken);

            return produtos.Select(p => new PedidoDTO
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
