namespace Workbalance.Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        public string PreferredLanguage { get; set; } = "pt-BR";

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

}
