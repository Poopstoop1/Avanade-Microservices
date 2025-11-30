using Domain.Enums;
using Domain.ValueObjects;
using Domain.Events;


namespace Domain.Entities
{
    public class Pedido : AggregateRoot
    {
        public Guid UsuarioId { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;

        public Preco ValorTotal { get; private set; } = default!;

        public PedidoStatus Status { get; set; } = PedidoStatus.Criado;

        public List<PedidoItem> Itens { get; private set; } = new List<PedidoItem>();

        public Pedido(Guid usuarioId, List<PedidoItem> itens)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            Itens = itens;
            ValorTotal = new Preco(itens.Sum(i => i.Subtotal));
            var itensDto = Itens.Select(i => new PedidoCriadoEvent.PedidoItemDto(i.ProdutoId, i.Quantidade)).ToList();
            AddDomainEvent(new PedidoCriadoEvent(Id, itensDto));
        }
        private Pedido() { }


        public void PedidoConfirmado()
        {
            Status = PedidoStatus.Confirmado;
            var itensDto = Itens.Select(i => new PedidoConfirmadoEvent.PedidoItemDto(i.ProdutoId, i.Quantidade)).ToList();
            AddDomainEvent(new PedidoConfirmadoEvent(Id, UsuarioId, itensDto));
        }

        public void PedidoCancelado() {
            Status = PedidoStatus.Cancelado;
        }



    }
}