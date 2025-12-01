using Application.DTOs.ViewModels;
using Domain.Enums;
using MediatR;


namespace Application.Queries
{
    public class GetByStatusPedido : IRequest<List<PedidoViewDTO>>
    {

        public PedidoStatus Status { get; set; }
        public GetByStatusPedido(PedidoStatus status)
        {
            Status = status;
        }
    }
}
