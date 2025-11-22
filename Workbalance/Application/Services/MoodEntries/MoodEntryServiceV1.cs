using Workbalance.Application.Dtos;
using Workbalance.Domain.Entity;
using Workbalance.Infrastructure.Repository;

namespace Workbalance.Application.Services.MoodEntries
{
    public class MoodEntryServiceV1 : IMoodEntryService
    {
        private readonly IRepository<MoodEntry> _repo;

        public MoodEntryServiceV1(IRepository<MoodEntry> repo)
        {
            _repo = repo;
        }

        public async Task<MoodEntryResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : ToResponse(entity);
        }

        public async Task<IEnumerable<MoodEntryResponseDto>> GetAllByUserAsync(Guid userId)
        {
            var all = await _repo.GetAllAsync();
            return all
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Date)
                .Select(ToResponse);
        }

        public async Task<MoodEntryResponseDto> CreateAsync(MoodEntryCreateDto dto)
        {
            var existing = (await _repo.GetAllAsync())
                .FirstOrDefault(m => m.UserId == dto.UserId && m.Date == dto.Date);

            if (existing != null)
                throw new Exception("Já existe um registro de humor para essa data.");

            var entity = new MoodEntry
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Date = dto.Date,
                Mood = dto.Mood,
                Stress = dto.Stress,
                Productivity = dto.Productivity,
                Notes = dto.Notes,
                Tags = dto.Tags,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return ToResponse(entity);
        }

        public async Task<MoodEntryResponseDto?> UpdateAsync(Guid id, MoodEntryUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            if (dto.Mood.HasValue)
                entity.Mood = dto.Mood.Value;

            if (dto.Stress.HasValue)
                entity.Stress = dto.Stress.Value;

            if (dto.Productivity.HasValue)
                entity.Productivity = dto.Productivity.Value;

            if (dto.Notes != null)
                entity.Notes = dto.Notes;

            if (dto.Tags != null)
                entity.Tags = dto.Tags;

            entity.UpdatedAt = DateTime.UtcNow;

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

        private static MoodEntryResponseDto ToResponse(MoodEntry m)
            => new(
                m.Id,
                m.UserId,
                m.Date,
                m.Mood,
                m.Stress,
                m.Productivity,
                m.Notes,
                m.Tags,
                m.CreatedAt,
                m.UpdatedAt
            );
    }
}
