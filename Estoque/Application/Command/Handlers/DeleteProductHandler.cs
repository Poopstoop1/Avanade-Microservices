using Domain.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProduct, Unit>
    {
        private readonly IProdutoRepository _produtoRepository;

        public DeleteProductHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto com Id {request.Id} não encontrado.");
            }
            await _produtoRepository.Delete(produto, cancellationToken);
            
            return Unit.Value;
        }
    }
}
