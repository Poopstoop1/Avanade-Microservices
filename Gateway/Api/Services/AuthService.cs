using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public class AuthService
    {
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IUsuarioRepository _repo;
        public AuthService(IPasswordHasher<Usuario> passwordHasher, IUsuarioRepository repo)
        {
            _passwordHasher = passwordHasher;
            _repo = repo;
        }
        public async Task Register(string name, string email, string role, string password, CancellationToken cancellationToken)
        {
            var user = new Usuario
            {
                Name = name,
                Email = email,
                Role = role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _repo.AddSync(user, cancellationToken);
        }

        public async Task<Usuario?> Login(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByEmailAsync(email, cancellationToken);

            _ = user ?? throw new Exception("Email não encontrado");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

    }
}
