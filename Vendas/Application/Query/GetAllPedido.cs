using Application.DTOs;
using MediatR;


namespace Application.Query
{
    public class GetByAllPedido : IRequest<List<PedidoDTO>>
    {
        public int Id { get; set; }
    }
}
