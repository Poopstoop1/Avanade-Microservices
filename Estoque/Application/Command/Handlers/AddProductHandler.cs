using Domain.IRepository;
using MediatR;
using Domain.Entities;

namespace Application.Command.Handlers
{
    public class AddProductHandler(IProdutoRepository produtoRepository) : IRequestHandler<AddProduct, Guid>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

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
