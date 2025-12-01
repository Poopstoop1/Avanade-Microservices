using MediatR;

namespace Application.Command
{
    public class DeletePedido(Guid id) : IRequest<Unit>
    {

        public Guid Id { get; private set; } = id;
    }
}
