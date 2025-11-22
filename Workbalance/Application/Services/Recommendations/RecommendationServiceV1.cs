using Workbalance.Application.Dtos;
using Workbalance.Domain.Entity;
using Workbalance.Infrastructure.Repository;

namespace Workbalance.Application.Services.Recommendations
{
    public class RecommendationServiceV1 : IRecommendationService
    {
        private readonly IRepository<Recommendation> _repo;

        public RecommendationServiceV1(IRepository<Recommendation> repo)
        {
            _repo = repo;
        }

        public async Task<RecommendationResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : ToResponse(entity);
        }

        public async Task<IEnumerable<RecommendationResponseDto>> GetAllByUserAsync(Guid userId)
        {
            var all = await _repo.GetAllAsync();
            return all
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(ToResponse);
        }

        public async Task<RecommendationResponseDto> CreateAsync(RecommendationCreateDto dto)
        {
            var entity = new Recommendation
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Type = dto.Type,
                Message = dto.Message,
                ActionUrl = dto.ActionUrl,
                ScheduledAt = dto.ScheduledAt,
                Source = dto.Source,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return ToResponse(entity);
        }

        public async Task<RecommendationResponseDto?> UpdateAsync(Guid id, RecommendationUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            if (dto.Type.HasValue)
                entity.Type = dto.Type.Value;

            if (dto.Message != null)
                entity.Message = dto.Message;

            if (dto.ActionUrl != null)
                entity.ActionUrl = dto.ActionUrl;

            if (dto.ScheduledAt.HasValue)
                entity.ScheduledAt = dto.ScheduledAt.Value;

            if (dto.Source.HasValue)
                entity.Source = dto.Source.Value;

            _repo.Update(entity);
            await _repo.SaveChangesAsync();

            return ToResponse(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _repo.Delete(entity);
            await _repo.SaveChangesAsync();

            return true;
        }

        private static RecommendationResponseDto ToResponse(Recommendation r)
            => new(
                r.Id,
                r.UserId,
                r.Type,
                r.Message,
                r.ActionUrl,
                r.ScheduledAt,
                r.Source,
                r.CreatedAt
            );
    }
}
