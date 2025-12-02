using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.IRepository;


namespace Infrastructure.Repositories
{
    public class PedidoRepository(VendasDBContext context) : IPedidoRepository
    {
        private readonly VendasDBContext _context = context;

        public async Task AddAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            await _context.Pedidos.AddAsync(pedido, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task<List<Pedido>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Pedidos.Include(p => p.Itens).ToListAsync(cancellationToken);
        }

        public async Task<Pedido?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Pedidos.
                 Include(p => p.Itens).
                 FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Pedido>> GetByStatusAsync(PedidoStatus status, CancellationToken cancellationToken)
        {
            return await _context.Pedidos
                .Where(p => p.Status == status)
                .Include(p => p.Itens)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
