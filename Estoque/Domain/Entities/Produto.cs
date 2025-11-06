using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public decimal Preco { get; set; } = default!;
        public int Quantidade { get; set; } = default!;
    }
}