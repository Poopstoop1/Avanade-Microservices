using Application.DTOs.ViewModels;
using MediatR;

namespace Application.Queries
{
    public class GetByIdPedido : IRequest<PedidoViewDTO>
    {
        
        public GetByIdPedido(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }

    }
}
