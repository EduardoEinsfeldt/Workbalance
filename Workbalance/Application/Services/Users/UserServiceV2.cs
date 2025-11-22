using Workbalance.Application.Dtos;
using Workbalance.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Workbalance.Infrastructure.Repository;

namespace Workbalance.Application.Services.Users
{
    public class UserServiceV2 : IUserService
    {
        private readonly IRepository<User> _repo;
        private readonly PasswordHasher<User> _hasher = new();

        public UserServiceV2(IRepository<User> repo)
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
            // Hash da senha
            string hashedPassword = _hasher.HashPassword(new User(), dto.Password);

            var parametros = new Dictionary<string, object>
            {
                { "p_nm_name", dto.Name },
                { "p_ds_email", dto.Email },
                { "p_ds_password_hash", hashedPassword },
                { "p_ds_preferred_language", dto.PreferredLanguage ?? "pt-BR" },
                { "p_cd_user_id", null! } // OUT
            };

            // Executa a procedure
            await _repo.ExecutarProcedureAsync("PKG_WORKBALANCE.PRC_INSERT_USER", parametros);

            // Recupera o ID retornado pelo Oracle
            var returnedIdRaw = parametros["p_cd_user_id"]?.ToString();
            if (returnedIdRaw == null)
                throw new Exception("Procedure PKG_WORKBALANCE.PRC_INSERT_USER did not return the user ID.");

            Guid newUserId = Guid.Parse(returnedIdRaw);

            // Recupera o usuário recém criado para retornar o DTO completo
            var createdUser = await _repo.GetByIdAsync(newUserId)
                ?? throw new Exception("User created by procedure but not found afterwards.");

            return ToResponse(createdUser);
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

        // Conversão Entity → DTO
        private static UserResponseDto ToResponse(User u)
            => new(
                u.Id,
                u.Name,
                u.Email,
                u.PreferredLanguage,
                u.CreatedAt,
                u.UpdatedAt
            );
    }
}
