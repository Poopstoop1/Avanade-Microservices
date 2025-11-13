using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class PedidoConfirmadoEvent : IDomainEvent
    {
        public Guid PedidoId { get; }

        public PedidoConfirmadoEvent(Guid pedidoId)
        {
            PedidoId = pedidoId;
        }
    }
}
