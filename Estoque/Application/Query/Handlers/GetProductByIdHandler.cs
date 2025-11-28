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
    public class GetProductByIdHandler : IRequestHandler<GetProductById, ProdutoDTO>
    {
        private readonly IProdutoRepository _produtoRepository;

        public GetProductByIdHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<ProdutoDTO> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto com Id {request.Id} não encontrado.");
            }

            return new ProdutoDTO
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco.Valor,
                Quantidade = produto.Quantidade,
            };

        }
    }
}
