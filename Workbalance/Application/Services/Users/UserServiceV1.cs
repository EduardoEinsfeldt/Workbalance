using Workbalance.Application.Dtos;
using Workbalance.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Workbalance.Infrastructure.Repository;

namespace Workbalance.Application.Services.Users
{
    public class UserServiceV1 : IUserService
    {
        private readonly IRepository<User> _repo;
        private readonly PasswordHasher<User> _hasher = new();

        public UserServiceV1(IRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<UserResponseDto?> GetByIdAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : ToResponse(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(ToResponse);
        }

        public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PreferredLanguage = dto.PreferredLanguage ?? "pt-BR",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return ToResponse(user);
        }

        public async Task<UserResponseDto?> UpdateAsync(Guid id, UserUpdateDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return null;

            if (dto.Name != null)
                user.Name = dto.Name;

            if (dto.Email != null)
                user.Email = dto.Email;

            if (dto.Password != null)
                user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            if (dto.PreferredLanguage != null)
                user.PreferredLanguage = dto.PreferredLanguage;

            user.UpdatedAt = DateTime.UtcNow;

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return ToResponse(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;

            _repo.Delete(user);
            await _repo.SaveChangesAsync();

            return true;
        }

        private static UserResponseDto ToResponse(User u)
            => new(u.Id, u.Name, u.Email, u.PreferredLanguage, u.CreatedAt, u.UpdatedAt);
    }
}
