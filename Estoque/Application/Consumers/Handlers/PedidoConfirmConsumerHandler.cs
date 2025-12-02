using Domain.Events;
using Domain.IRepository;
using MediatR;


namespace Application.Consumers.Handlers
{
    public class PedidoConfirmConsumerHandler : IRequestHandler<PedidoConfirmadoEvent,Unit>
    {

        private readonly IProdutoRepository _produtoRepository;

        public PedidoConfirmConsumerHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Unit> Handle(PedidoConfirmadoEvent notification, CancellationToken cancellationToken)
        {

            foreach (var item in notification.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Produto {item.ProdutoId} não encontrado.");

                produto.ConfirmarReserva(item.Quantidade);

                await _produtoRepository.Update(produto, cancellationToken);
            }

            return Unit.Value;
        }

    }
}
