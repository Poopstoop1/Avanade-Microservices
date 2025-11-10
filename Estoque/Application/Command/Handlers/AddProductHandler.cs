using Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Command.Handlers
{
    public class AddProductHandler : IRequestHandler<AddProduct, Guid>
    {
        private readonly IProdutoRepository _produtoRepository;

        public AddProductHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Guid> Handle(AddProduct request, CancellationToken cancellationToken)
        {

            var produto = new Produto
            (
                request.Nome,
                request.Descricao,
                request.Preco,
                request.Quantidade
            );

            await  _produtoRepository.AddAsync(produto, cancellationToken);

            return produto.Id;
        }
    }
}
