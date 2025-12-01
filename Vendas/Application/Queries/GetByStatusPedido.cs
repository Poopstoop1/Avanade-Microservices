using Application.DTOs.ViewModels;
using Domain.Enums;
using MediatR;


namespace Application.Queries
{
    public class GetByStatusPedido(PedidoStatus status) : IRequest<List<PedidoViewDTO>>
    {
        public PedidoStatus Status { get; set; } = status;
      
    }
}
