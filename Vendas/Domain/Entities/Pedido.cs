using Domain.Enums;
using Domain.Events;
using Domain.ValueObjects;


namespace Domain.Entities
{
    public class Pedido : AggregateRoot
    {
        public Guid UsuarioId { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;

        public Preco ValorTotal { get; private set; } = default!;

        public PedidoStatus Status { get; set; } = PedidoStatus.Criado;

        public List<PedidoItem> Itens { get; private set; } = [];

        public Pedido(Guid usuarioId, List<PedidoItem> itens)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            Itens = itens;
            ValorTotal = new Preco(itens.Sum(i => i.Subtotal));
            List<PedidoItemDto> itensDto = 
                [
                    ..Itens.Select(i => new PedidoItemDto(i.ProdutoId, i.Quantidade))
                ];
         
            AddDomainEvent(new PedidoCriadoEvent(Id, itensDto));
        }
        private Pedido() { }


        public void PedidoConfirmado()
        {
            Status = PedidoStatus.Confirmado;
            List<PedidoItemDto> itensDto = 
                [
                    ..Itens.Select(i => new PedidoItemDto(i.ProdutoId, i.Quantidade))
                ];
            AddDomainEvent(new PedidoConfirmadoEvent(Id, UsuarioId, itensDto));
        }

        public void PedidoCancelado() {
            Status = PedidoStatus.Cancelado;
        }



    }
}