using Application.DTOs.ViewModels;
using MediatR;

namespace Application.Queries
{
    public class GetByIdPedido(Guid id) : IRequest<PedidoViewDTO>
    {

        public Guid Id { get; set; } = id;

    }
}
