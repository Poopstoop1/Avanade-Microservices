using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class EstoqueRejeitadoEvent : IRequest
    {
            public Guid PedidoId { get; }
            public string Motivo { get; }

            public EstoqueRejeitadoEvent(Guid pedidoId, string motivo)
            {
                PedidoId = pedidoId;
                Motivo = motivo;
            }
  
    }
}
