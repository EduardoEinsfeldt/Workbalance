using Workbalance.Application.Dtos;
using Workbalance.Domain.Entity;
using Workbalance.Infrastructure.Repository;

namespace Workbalance.Application.Services.Recommendations
{
    public class RecommendationServiceV2 : IRecommendationService
    {
        private readonly IRepository<Recommendation> _repo;

        public RecommendationServiceV2(IRepository<Recommendation> repo)
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
            var parametros = new Dictionary<string, object>
            {
                { "p_cd_user_id", dto.UserId.ToString() },
                { "p_ds_type", dto.Type.ToString() },
                { "p_ds_message", dto.Message },
                { "p_ds_action_url", dto.ActionUrl },
                { "p_ts_scheduled_at", dto.ScheduledAt },
                { "p_ds_source", dto.Source.ToString() },
                { "p_cd_recommendation_id", null! } // OUT
            };

            await _repo.ExecutarProcedureAsync("PKG_WORKBALANCE.PRC_INSERT_RECOMMENDATION", parametros);

            var returnedId = parametros["p_cd_recommendation_id"]?.ToString()
                ?? throw new Exception("Procedure PKG_WORKBALANCE.PRC_INSERT_RECOMMENDATION não retornou o ID.");

            Guid newId = Guid.Parse(returnedId);

            var entity = await _repo.GetByIdAsync(newId)
                ?? throw new Exception("Recomendação criada via procedure, mas não encontrada depois.");

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
