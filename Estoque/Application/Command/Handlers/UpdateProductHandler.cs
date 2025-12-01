using Domain.IRepository;
using MediatR;

using Domain.ValueObjects;

namespace Application.Command.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProduct, Unit>
    {
        private readonly IProdutoRepository _produtoRepository;

        public UpdateProductHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Unit> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto com Id {request.Id} não encontrado.");
            }

            produto.Nome = request.Nome;
            produto.Descricao = request.Descricao;
            produto.Preco = new Preco( request.Preco);
            produto.Quantidade = request.Quantidade;

            await _produtoRepository.Update(produto, cancellationToken);
            return Unit.Value;
        }
    }
}
