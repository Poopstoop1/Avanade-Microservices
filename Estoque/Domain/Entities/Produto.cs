using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Produto

    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;
      
        public Preco Preco { get; private set; } = default!;
        public int Quantidade { get; set; } = default!;

        public Produto(string nome, string descricao, decimal preco, int quantidade)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Preco = new Preco(preco);
            Quantidade = quantidade;
        }
        private Produto() { }
    }
}