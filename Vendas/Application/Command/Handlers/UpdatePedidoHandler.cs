using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handlers
{
    public class UpdatePedidoHandler : IRequestHandler<UpdatePedido, Unit>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public UpdatePedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Unit> Handle(UpdatePedido request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (pedido == null)
            {
                throw new KeyNotFoundException($"Pedido com id {request.Id} não encontrado");
            }

            pedido.Status = request.Status;

            await _pedidoRepository.UpdateAsync(pedido, cancellationToken);
            return Unit.Value;
        }
    }
}
