namespace Application.DTOs
{
    public class PedidoItemDTO
    {
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
