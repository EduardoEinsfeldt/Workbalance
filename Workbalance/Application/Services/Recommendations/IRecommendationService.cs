using Workbalance.Application.Dtos;

namespace Workbalance.Application.Services.Recommendations
{
    public interface IRecommendationService
    {
        Task<RecommendationResponseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<RecommendationResponseDto>> GetAllByUserAsync(Guid userId);
        Task<RecommendationResponseDto> CreateAsync(RecommendationCreateDto dto);
        Task<RecommendationResponseDto?> UpdateAsync(Guid id, RecommendationUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
