using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var itens = request.Itens.Select(i => new PedidoItem(i.ProdutoId, i.NomeProduto, i.Quantidade, new Preco(i.PrecoUnitario))).ToList();

            var pedido = new Pedido(request.UsuarioId, itens);
            await _pedidoRepository.AddAsync(pedido,cancellationToken);
            return pedido.Id;
        }
    }
}
