using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class AddPedido(Guid usuarioId, List<PedidoItemDTO> itens) : IRequest<Guid>
    {

        public Guid UsuarioId { get; set; } = usuarioId;
     
        public List<PedidoItemDTO> Itens { get; set; } = itens;

    }
}
