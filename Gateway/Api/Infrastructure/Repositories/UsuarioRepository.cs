

using Api.Entities;
using Api.Infrastructure.Data;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly GatewayDbContext _context;

        public UsuarioRepository(GatewayDbContext context)
        {
            _context = context;
        }

        public async Task AddSync(Usuario usuario, CancellationToken cancellationToken)
        {
            await _context.Usuarios.AddAsync(usuario, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Usuario usuario, CancellationToken cancellationToken)
        {
           _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Usuario>> GetAllUsuarioAsync(CancellationToken cancellationToken)
        {
            return await _context.Usuarios.ToListAsync(cancellationToken);
        }
    }
}
