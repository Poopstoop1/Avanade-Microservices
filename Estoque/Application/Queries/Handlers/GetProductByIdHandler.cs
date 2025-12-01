using Application.DTOs.ViewModels;
using Domain.IRepository;
using MediatR;

namespace Application.Queries.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductById, ProdutoViewDTO>
    {
        private readonly IProdutoRepository _produtoRepository;

        public GetProductByIdHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<ProdutoViewDTO> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto com Id {request.Id} não encontrado.");
            }

            return new ProdutoViewDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco.Valor,
                Quantidade = produto.Quantidade,
                QuantidadeReservada = produto.QuantidadeReservada
            };

        }
    }
}
