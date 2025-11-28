using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Produto : AggregateRoot

    {
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;

        public Preco Preco { get; set; } = default!;
        public int Quantidade { get; set; } = default!;

        public int QuantidadeReservada { get; private set; }

        public Produto(string nome, string descricao, decimal preco, int quantidade)
        {
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