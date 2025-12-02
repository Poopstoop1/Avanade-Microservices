using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class EstoqueRejeitadoEvent(Guid pedidoId, string motivo) : IDomainEvent
    {
        public Guid PedidoId { get; } = pedidoId;
        public string Motivo { get; } = motivo;

       
    }
}
