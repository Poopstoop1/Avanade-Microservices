using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.ValueObjects;


namespace Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UsuarioId { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public Preco ValorTotal { get; set; } = default!;

        public PedidoStatus Status { get; set; } = PedidoStatus.Criado;

        public ICollection<PedidoItem> Itens { get; set; } = new List<PedidoItem>();

        public Pedido(Guid usuarioId, ICollection<PedidoItem> itens)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            Itens = itens;
            ValorTotal = new Preco(itens.Sum(i => i.Subtotal));
        }
        private Pedido() { }

    }
}