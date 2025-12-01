using Application.DTOs.ViewModels;
using MediatR;


namespace Application.Queries
{
    public class GetByAllPedido : IRequest<List<PedidoViewDTO>>
    {
    }
}
