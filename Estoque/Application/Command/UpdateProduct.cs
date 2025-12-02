using MediatR;

namespace Application.Command
{
    public class UpdateProduct(Guid id, string nome, string descricao, decimal preco, int quantidade) : IRequest<Unit>
    {


        public Guid Id { get; } = id;
        public string Nome { get;  } = nome;
        public string Descricao { get; } = descricao;
        public decimal Preco { get; } = preco;
        public int Quantidade { get; } = quantidade;
    }
}
