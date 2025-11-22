namespace Workbalance.Domain.Entity
{
    public class MoodEntry
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateOnly Date { get; set; }

        public int Mood { get; set; }
        public int Stress { get; set; }
        public int Productivity { get; set; }

        public string? Notes { get; set; }

        public string? Tags { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
