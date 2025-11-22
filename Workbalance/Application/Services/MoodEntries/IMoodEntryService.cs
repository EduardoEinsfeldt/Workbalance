using Workbalance.Application.Dtos;

namespace Workbalance.Application.Services.MoodEntries
{
    public interface IMoodEntryService
    {
        Task<MoodEntryResponseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<MoodEntryResponseDto>> GetAllByUserAsync(Guid userId);
        Task<MoodEntryResponseDto> CreateAsync(MoodEntryCreateDto dto);
        Task<MoodEntryResponseDto?> UpdateAsync(Guid id, MoodEntryUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}