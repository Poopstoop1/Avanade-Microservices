using MediatR;

namespace Application.Command
{
    public class DeletePedido : IRequest<Unit>
    {
        public DeletePedido(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
