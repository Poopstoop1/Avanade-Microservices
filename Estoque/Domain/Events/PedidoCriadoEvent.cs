

using MediatR;

namespace Domain.Events
{
    public class PedidoCriadoEvent : IRequest
    {
        public Guid PedidoId { get; set; }
        public List<PedidoItemDto> Items { get; set; } = [];
        public PedidoCriadoEvent(Guid pedidoId, List<PedidoItemDto> items)
        {
            PedidoId = pedidoId;
            Items = items;
        }
        public class PedidoItemDto
        {
            public Guid ProdutoId { get; set; }
            public int Quantidade { get; set; }
            public PedidoItemDto(Guid produtoId, int quantidade)
            {
                ProdutoId = produtoId;
                Quantidade = quantidade;
            }
        }
    }
}
