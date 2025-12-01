using Domain.Enums;
using MediatR;

namespace Application.Command
{
    public class UpdatePedido(Guid id, PedidoStatus status) : IRequest<Unit>
    {

        public Guid Id { get; private set; } = id;

        public PedidoStatus Status { get; set; } = status;

    }
}
