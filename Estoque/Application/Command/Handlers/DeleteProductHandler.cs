using Domain.IRepository;
using MediatR;

namespace Application.Command.Handlers
{
    public class DeleteProductHandler(IProdutoRepository produtoRepository) : IRequestHandler<DeleteProduct, Unit>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

       

        public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken) 
                ?? throw new KeyNotFoundException($"Produto com Id {request.Id} não encontrado.");
            
            await _produtoRepository.Delete(produto, cancellationToken);
            
            return Unit.Value;
        }
    }
}
