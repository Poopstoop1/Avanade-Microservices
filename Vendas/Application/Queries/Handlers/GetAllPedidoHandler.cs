using Application.DTOs;
using Application.DTOs.ViewModels;
using MediatR;

namespace Application.Queries.Handlers
{
    public class GetAllPedidoHandler(IPedidoRepository pedidoRepository) : IRequestHandler<GetByAllPedido, List<PedidoViewDTO>>
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

        public async Task<List<PedidoViewDTO>> Handle(GetByAllPedido request, CancellationToken cancellationToken)
        {
            var produtos = await _pedidoRepository.GetAllAsync(cancellationToken);

            return [
                ..produtos.Select(p => new PedidoViewDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    DataCriacao = p.DataCriacao,
                    ValorTotal = p.ValorTotal,
                    Status = p.Status,
                    Itens = [
                        ..p.Itens.Select(i => new PedidoItemDTO
                        {
                            ProdutoId = i.ProdutoId,
                            NomeProduto = i.NomeProduto,
                            Quantidade = i.Quantidade,
                            PrecoUnitario = i.PrecoUnitario
                        })
                        ],
                })
                ];
        }

    }
}
