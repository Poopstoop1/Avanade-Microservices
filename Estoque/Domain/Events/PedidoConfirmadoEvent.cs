using Domain.Events;
using MediatR;


namespace Domain.Events
{
    public class PedidoConfirmadoEvent : IRequest<Unit>
    {

        public Guid PedidoId { get; set; }
        public List<ItemPedidoEvent> Itens { get; set; } = [];
        public PedidoConfirmadoEvent(Guid pedidoID, List<ItemPedidoEvent> itens)
        {
            PedidoId = pedidoID;
            Itens = itens;
        }

    }
    public class ItemPedidoEvent
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public ItemPedidoEvent(Guid produtoId, int quantidade)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

    }
}
