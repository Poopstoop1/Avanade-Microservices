using MediatR;


namespace Domain.Events
{
    public class EstoqueRejeitadoEvent(Guid pedidoId, string motivo) : IRequest
    {
        public Guid PedidoId { get; } = pedidoId;
        public string Motivo { get; } = motivo;

    }
}
