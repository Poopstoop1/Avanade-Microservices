using Application.DTOs;
using Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProducts, List<ProdutoDTO>>
    {
        private readonly IProdutoRepository produtoRepository;

        public GetAllProductsHandler(IProdutoRepository produtoRepository)
        {
            this.produtoRepository = produtoRepository;
        }

        public async Task<List<ProdutoDTO>> Handle(GetAllProducts request, CancellationToken cancellationToken)
        {

            var produtos = await produtoRepository.GetAllAsync(cancellationToken);

            return produtos.Select(produto => new ProdutoDTO
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco.Valor,
                Quantidade = produto.Quantidade
            }).ToList();

        }
    }
}
