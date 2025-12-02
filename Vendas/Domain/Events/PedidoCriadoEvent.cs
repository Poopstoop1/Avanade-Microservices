
namespace Domain.Events
{
    public class PedidoCriadoEvent(Guid pedidoId, List<PedidoItemDto> items) : IDomainEvent
    {
        public Guid PedidoId { get; } = pedidoId;

        public List<PedidoItemDto> Items { get; set; } = items;


    }
    public class PedidoItemDto(Guid produtoId, int quantidade)
    {
        public Guid ProdutoId { get; set; } = produtoId;
        public int Quantidade { get; set; } = quantidade;   

    }
}
