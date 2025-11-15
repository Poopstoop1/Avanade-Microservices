namespace Domain.Events
{
    public class PedidoConfirmadoEvent : IDomainEvent
    {
        public Guid PedidoId { get; }
        public Guid UsuarioId { get; }

        public List<PedidoItemDto> Itens { get; }

        public PedidoConfirmadoEvent(Guid pedidoId, Guid usuarioId, List<PedidoItemDto> itens)
        {
            PedidoId = pedidoId;
            UsuarioId = usuarioId;
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
