using Workbalance.Domain.Enums;

namespace Workbalance.Domain.Entity
{
    public class Recommendation
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public RecommendationType Type { get; set; }

        public string Message { get; set; } = default!;

        public string? ActionUrl { get; set; }

        public DateTime? ScheduledAt { get; set; }

        public RecommendationSource Source { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
