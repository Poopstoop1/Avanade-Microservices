
using Application.Interfaces;
using Domain.Events;
using Domain.IRepository;
using MediatR;

namespace Application.Event.Handlers
{
    public class PedidoCriadoHandler : IRequestHandler<PedidoCriadoEvent>
    {

        private readonly IProdutoRepository _repo;
        private readonly IMessageBusClient _bus;

        public PedidoCriadoHandler(IProdutoRepository repo, IMessageBusClient bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task Handle(PedidoCriadoEvent request, CancellationToken cancellationToken)
        {
            try
            {
               
                foreach (var item in request.Itens)
                {
                    var produto = await _repo.GetByIdAsync(item.ProdutoId, cancellationToken);

                    if (produto is null) 
                        throw new InvalidOperationException($"Produto {item.ProdutoId} não encontrado no estoque."); 
                    
                    var estoqueLivre = produto.Quantidade - produto.QuantidadeReservada;

                    if (estoqueLivre < item.Quantidade)
                        throw new InvalidOperationException($"Produto {item.ProdutoId} não possui estoque suficiente.");

                    produto.Reservar(item.Quantidade);

                    await _repo.Update(produto, cancellationToken);
                }
               
                await Task.CompletedTask;
            }
            catch (InvalidOperationException ex)
            {
                await _bus.Publish(
                    new EstoqueRejeitadoEvent(request.PedidoId, ex.Message),
                    routingKey: "estoque-indisponivel",
                    exchange: "pedido.exchange",
                    cancellationToken);
            }

        }
    }
}
