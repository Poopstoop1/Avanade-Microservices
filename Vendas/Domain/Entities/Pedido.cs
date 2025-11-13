using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.ValueObjects;


namespace Domain.Entities
{
    public class Pedido : AgreggateRoot
    {
        public Guid UsuarioId { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;

        public Preco ValorTotal { get; private set; } = default!;

        public PedidoStatus Status { get; set; } = PedidoStatus.Criado;

        public List<PedidoItem> Itens { get; private  set; } = new List<PedidoItem>();

        public Pedido(Guid usuarioId, List<PedidoItem> itens)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            Itens = itens;
            ValorTotal = new Preco(itens.Sum(i => i.Subtotal));
        }
        private Pedido() { }

    }
}