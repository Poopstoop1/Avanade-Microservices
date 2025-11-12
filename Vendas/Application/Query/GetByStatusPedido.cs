using Application.DTOs;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query
{
    public class GetByStatusPedido : IRequest<List<PedidoDTO>>
    {

        public PedidoStatus Status { get; set; }
        public GetByStatusPedido(PedidoStatus status)
        {
            Status = status;
        }
    }
}
