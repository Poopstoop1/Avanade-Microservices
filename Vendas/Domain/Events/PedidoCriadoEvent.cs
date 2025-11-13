using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class PedidoCriadoEvent : IDomainEvent
    {
        public Guid PedidoId { get; }
        public Guid UsuarioId { get; }

        public PedidoCriadoEvent(Guid pedidoId, Guid usuarioId)
        {
            PedidoId = pedidoId;
            UsuarioId = usuarioId;
        }
    }
}
