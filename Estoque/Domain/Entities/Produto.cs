using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Produto : AggregateRoot

    {
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;

        public Preco Preco { get; set; } = default!;
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

        public void SubtrairEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new Exception("Quantidade deve ser maior que zero.");

            if (Quantidade < quantidade)
                throw new Exception("Estoque insuficiente.");

            Quantidade -= quantidade;
        }
    }
}