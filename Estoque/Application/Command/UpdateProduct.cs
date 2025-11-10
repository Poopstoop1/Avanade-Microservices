using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class UpdateProduct : IRequest<Unit>
    {

        public UpdateProduct(Guid id,string nome, string descricao, decimal preco,  int quantidade)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Quantidade = quantidade;
        }

        public Guid Id { get; }
        public string Nome { get;  } = default!;
        public string Descricao { get; } = default!;
        public decimal Preco { get; }
        public int Quantidade { get;  }
    }
}
