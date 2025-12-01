using Domain.Entities;
using Domain.ValueObjects;
using MediatR;


namespace Application.Command.Handlers
{
    public class AddPedidoHandler(IPedidoRepository pedidoRepository) : IRequestHandler<AddPedido, Guid>
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

        public async Task<Guid> Handle(AddPedido request, CancellationToken cancellationToken)
        {
            List<PedidoItem> itens =
            [
                ..request.Itens.Select(i =>
                    new PedidoItem(i.ProdutoId, i.NomeProduto, i.Quantidade, new Preco(i.PrecoUnitario))
                )
            ];

            var pedido = new Pedido(request.UsuarioId, itens);
            await _pedidoRepository.AddAsync(pedido,cancellationToken);
            return pedido.Id;
        }
    }
}
