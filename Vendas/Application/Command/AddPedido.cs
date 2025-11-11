using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Command
{
    public class AddPedido : IRequest<Guid>
    {
        public AddPedido(Guid usuarioId, DateTime dataCriacao, decimal valorTotal, PedidoStatus status, List<PedidoItem> itens)
        {
            UsuarioId = usuarioId;
            DataCriacao = dataCriacao;
            ValorTotal = valorTotal;
            Status = status;
            Itens = itens;
        }

        public Guid UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal ValorTotal { get; set; }
        public PedidoStatus Status { get; set; }
        public List<PedidoItem> Itens { get; set; } = new List<PedidoItem>();

    }
}
