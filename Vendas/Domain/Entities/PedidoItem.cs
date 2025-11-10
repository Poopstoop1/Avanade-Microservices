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
        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; } = default!;

        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;

        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal => Quantidade * PrecoUnitario;

        protected PedidoItem() { }

        public PedidoItem(Guid produtoId, string nomeProduto, int quantidade, Preco preco)
        {
            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            PrecoUnitario = preco.Valor;
        }
    
    }
}