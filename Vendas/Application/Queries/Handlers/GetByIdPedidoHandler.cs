using Application.DTOs;
using Application.DTOs.ViewModels;
using MediatR;
using Domain.IRepository;
using Application.Interfaces;

namespace Application.Queries.Handlers
{
    public class GetByIdPedidoHandler(IPedidoRepository pedidoRepository, ICacheService cacheService) : IRequestHandler<GetByIdPedido, PedidoViewDTO>
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

        private readonly ICacheService _cacheService = cacheService;

        public async Task<PedidoViewDTO> Handle(GetByIdPedido request, CancellationToken cancellationToken)
        {
            var cacheKey = request.Id.ToString();

            var cachedPedido = await _cacheService.GetAsync<PedidoViewDTO>(cacheKey);

            if (cachedPedido == null)
            {
                var pedidoFromRepo = await _pedidoRepository.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Pedido com Id {request.Id} não encontrado.");
                var pedidoDto = new PedidoViewDTO
                {
                    Id = pedidoFromRepo.Id,
                    UsuarioId = pedidoFromRepo.UsuarioId,
                    DataCriacao = pedidoFromRepo.DataCriacao,
                    ValorTotal = pedidoFromRepo.ValorTotal,
                    Status = pedidoFromRepo.Status,
                    Itens = [
                        ..pedidoFromRepo.Itens.Select(i => new PedidoItemDTO {
                        ProdutoId = i.ProdutoId,
                        NomeProduto = i.NomeProduto,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario
                        }
                    )
                    ]
                };
                await _cacheService.SetAsync(cacheKey, pedidoDto);
                return pedidoDto;
            }

            return cachedPedido;
        }

    }
}
