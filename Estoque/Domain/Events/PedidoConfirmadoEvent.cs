using Domain.Events;
using MediatR;


namespace Domain.Events
{
    public class PedidoConfirmadoEvent(Guid pedidoID, List<ItemPedidoEvent> itens) : IRequest<Unit>
    {

        public Guid PedidoId { get; set; } = pedidoID;
        public List<ItemPedidoEvent> Itens { get; set; } = itens;
    

    }
    public class ItemPedidoEvent(Guid produtoId, int quantidade)
    {
        public Guid ProdutoId { get; set; } = produtoId;
        public int Quantidade { get; set; } = quantidade;

    }
}
