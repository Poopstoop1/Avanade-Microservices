using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.IRepository
{
    public interface IProdutoRepository
    {
        Task AddAsync(Produto produto, CancellationToken cancellationToken = default! );

        Task<Produto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default!);
        Task<List<Produto>> GetAllAsync(CancellationToken cancellationToken = default!);
        Task Update(Produto produto, CancellationToken cancellationToken = default!);
        Task Delete(Produto produto, CancellationToken cancellationToken = default!);

    }
}