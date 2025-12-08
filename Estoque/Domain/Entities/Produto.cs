using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Produto : AggregateRoot

    {
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;

        public Preco Preco { get; set; } = default!;
        public int Quantidade { get; set; } = default!;

        public int QuantidadeReservada { get; private set; } = default!;

        public Produto(string nome, string descricao, decimal preco, int quantidade)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio ou nulo", nameof(nome));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia ou nula", nameof(descricao));

            if (quantidade < 0)
                throw new ArgumentException("Quantidade não pode ser negativa", nameof(quantidade));

            if(preco < 0)
                throw new ArgumentException("Preço não pode ser negativo", nameof(preco));

            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Preco = new Preco(preco);
            Quantidade = quantidade;
        }
        private Produto() { }

        public void Reservar(int quantidade)
        {
            QuantidadeReservada += quantidade;
        }

        public void ConfirmarReserva(int quantidade)
        {
            QuantidadeReservada -= quantidade;
            Quantidade -= quantidade;
        }

        public void CancelarReserva(int quantidade)
        {
            QuantidadeReservada -= quantidade;
        }
    }
}