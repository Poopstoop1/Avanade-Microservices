using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class AddProduct(string nome, string descricao, decimal preco, int quantidade) : IRequest<Guid>
    {
        public string Nome { get; set; } = nome;
        public string Descricao { get; set; } = descricao;
        public decimal Preco { get; set; } = preco;
        public int Quantidade { get; set; } = quantidade;

    }
}
