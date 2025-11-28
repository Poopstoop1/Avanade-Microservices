

using MediatR;

namespace Domain.Events
{
    public class PedidoCriadoEvent : IRequest
    {
        public Guid PedidoId { get; }
        public List<PedidoItemDto> Itens { get; } = [];
        public PedidoCriadoEvent(Guid pedidoId, List<PedidoItemDto> itens)
        {
            PedidoId = pedidoId;
            Itens = itens;
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
