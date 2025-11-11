using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
  public class ProdutoRepository : IProdutoRepository
  {
        private readonly EstoqueDBContext _context;

        public ProdutoRepository(EstoqueDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Produto produto, CancellationToken cancellationToken = default)
        {
            
            await _context.Produtos.AddAsync(produto, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task Delete(Produto produto, CancellationToken cancellationToken = default)
        {
               _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Produto>> GetAllAsync(CancellationToken cancellationToken = default!)
        {
            return await _context.Produtos.ToListAsync(cancellationToken);
        }

        public async Task<Produto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
              return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task Update(Produto produto, CancellationToken cancellationToken = default)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
  
  
}