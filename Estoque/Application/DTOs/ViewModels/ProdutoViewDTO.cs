
namespace Application.DTOs.ViewModels
{
    public class ProdutoViewDTO
    {

        public Guid Id { get; set; } = default!;
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public decimal Preco { get; set; } = default!;
        public int Quantidade { get; set; } = default!;
        public int QuantidadeReservada { get; set; } = default!;

    }
}
