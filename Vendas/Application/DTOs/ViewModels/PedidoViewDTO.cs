using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTOs.ViewModels
{
    public class PedidoViewDTO
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public PedidoStatus Status { get; set; }

        public DateTime DataCriacao { get; set; }

        public Preco ValorTotal { get; set; } = default!;

        public List<PedidoItemDTO> Itens { get; set; } = [];
    }
}
