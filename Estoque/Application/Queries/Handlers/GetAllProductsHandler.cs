using Application.DTOs.ViewModels;
using Domain.IRepository;
using MediatR;

namespace Application.Queries.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProducts, List<ProdutoViewDTO>>
    {
        private readonly IProdutoRepository produtoRepository;

        public GetAllProductsHandler(IProdutoRepository produtoRepository)
        {
            this.produtoRepository = produtoRepository;
        }

        public async Task<List<ProdutoViewDTO>> Handle(GetAllProducts request, CancellationToken cancellationToken)
        {

            var produtos = await produtoRepository.GetAllAsync(cancellationToken);

            return produtos.Select(produto => new ProdutoViewDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco.Valor,
                Quantidade = produto.Quantidade,
                QuantidadeReservada = produto.QuantidadeReservada
            }).ToList();

        }
    }
}
