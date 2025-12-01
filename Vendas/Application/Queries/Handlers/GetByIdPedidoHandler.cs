using Application.DTOs;
using Application.DTOs.InputModels;
using Application.DTOs.ViewModels;
using MediatR;


namespace Application.Queries.Handlers
{
    public class GetByIdPedidoHandler : IRequestHandler<GetByIdPedido, PedidoViewDTO>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetByIdPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoViewDTO> Handle(GetByIdPedido request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw  new KeyNotFoundException($"Pedido com Id {request.Id} não encontrado.");

            return new PedidoViewDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                DataCriacao = pedido.DataCriacao,
                ValorTotal = pedido.ValorTotal,
                Status = pedido.Status,
                Itens = [
                    ..pedido.Itens.Select(i => new PedidoItemDTO {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                    }
                )
                ]
            };

        }

    }
}
