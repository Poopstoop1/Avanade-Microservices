using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query
{
    public class GetByIdPedido : IRequest<PedidoDTO>
    {
        
        public GetByIdPedido(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }

    }
}
