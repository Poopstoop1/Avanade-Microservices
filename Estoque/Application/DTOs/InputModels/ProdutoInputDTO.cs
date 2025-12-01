
namespace Application.DTOs.InputModels
{
    public class ProdutoInputDTO
    {
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public decimal Preco { get; set; } = default!;
        public int Quantidade { get; set; } = default!;
    }
}