using Application.DTOs;
using Domain.Entities;

using MediatR;

namespace Application.Command
{
    public class AddPedido : IRequest<Guid>
    {
        public AddPedido(Guid usuarioId, List<PedidoItemDTO> itens)
        {
            UsuarioId = usuarioId;
            Itens = itens;
        }

        public Guid UsuarioId { get; set; }
     
        public List<PedidoItemDTO> Itens { get; set; } = new List<PedidoItemDTO>();

    }
}
