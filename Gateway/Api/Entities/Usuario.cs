namespace Api.Entities
{
    public class Usuario
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; } = DateTime.UtcNow;

    }
}
