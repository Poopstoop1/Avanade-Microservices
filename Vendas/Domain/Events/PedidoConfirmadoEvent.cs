namespace Domain.Events
{
    public class PedidoConfirmadoEvent : IDomainEvent
    {
        public Guid PedidoId { get; set; }
        public Guid UsuarioId { get; set; }

        public List<PedidoItemDto> Itens { get; set; } = [];


        public PedidoConfirmadoEvent(Guid pedidoId, Guid usuarioId, List<PedidoItemDto> itens)
        {
            PedidoId = pedidoId;
            UsuarioId = usuarioId;
            Itens = itens;
        }
        public PedidoConfirmadoEvent() { }

    }
}
