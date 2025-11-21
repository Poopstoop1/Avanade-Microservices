using Api.Entities;

namespace Api.Interfaces
{
    public interface IUsuarioRepository
    {
        Task AddSync(Usuario usuario, CancellationToken cancellationToken);
        Task<List<Usuario>> GetAllUsuarioAsync(CancellationToken cancellationToken);

        Task<Usuario?>  GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task Delete(Usuario usuario, CancellationToken cancellationToken);
    }
}
