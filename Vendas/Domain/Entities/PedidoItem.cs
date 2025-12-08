using Domain.ValueObjects;

namespace Domain.Entities
{
    public class PedidoItem : IEntityBase
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid PedidoId { get; private set; }
        public Pedido? Pedido { get; private set; } = default!;

        public Guid ProdutoId { get; private set; }
        public string NomeProduto { get; private set; } = string.Empty;

        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        public decimal Subtotal => Quantidade * PrecoUnitario;

        public PedidoItem() { }

        public PedidoItem(Guid produtoId, string nomeProduto, int quantidade, Preco preco)
        {
            if (produtoId == Guid.Empty)
                throw new ArgumentException("Produto inválido.");

            if (string.IsNullOrWhiteSpace(nomeProduto))
                throw new ArgumentException("Nome do produto é obrigatório.");

            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.");


            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            PrecoUnitario = preco.Valor;

            if(PrecoUnitario < 0)
                throw new ArgumentException("Preco Unitario deve ser maior que zero.");
        }
    
    }
}