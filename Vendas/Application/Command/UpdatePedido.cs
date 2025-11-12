using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class UpdatePedido : IRequest<Unit>
    {
          
        public UpdatePedido(Guid id, PedidoStatus status)
        {
            Id = id;
  
            Status = status;
        }

        public Guid Id { get; private set; }

        public PedidoStatus Status { get; set; }

    }
}
