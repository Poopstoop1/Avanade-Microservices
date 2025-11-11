using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PedidoDTO
    {
        public Guid UsuarioId { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public Preco ValorTotal { get; set; } = default!;

        public PedidoStatus Status { get; set; } = PedidoStatus.Criado;

        public List<PedidoItem> Itens { get; set; } = new List<PedidoItem>();

    }
}