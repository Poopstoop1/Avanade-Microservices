using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Command.Handlers
{
    public class AddPedidoHandler : IRequestHandler<AddPedido, Guid>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public AddPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Guid> Handle(AddPedido request, CancellationToken cancellationToken)
        {
            var pedido = new Pedido(request.UsuarioId, request.Itens);
            await _pedidoRepository.AddAsync(pedido,cancellationToken);
            return pedido.Id;
        }
    }
}
