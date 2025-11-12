using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class PedidoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
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
            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            PrecoUnitario = preco.Valor;
        }
    
    }
}