namespace Application.DTOs.InputModels
{
    public class PedidoInputDTO
    {
        public Guid UsuarioId { get; set; }

        public List<PedidoItemDTO> Itens { get; set; } = [];

    }
}