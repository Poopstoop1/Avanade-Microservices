using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class AddProduct : IRequest<Guid>
    {

        public AddProduct(string nome, string descricao, decimal preco, int quantidade)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Quantidade = quantidade;
        }

        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;

        public decimal Preco { get; set; } = default!;
        public int Quantidade { get; set; } = default!;

    }
}
