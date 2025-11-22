using Workbalance.Application.Dtos;

namespace Workbalance.Application.Services.Users
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto> CreateAsync(UserCreateDto dto);
        Task<UserResponseDto?> UpdateAsync(Guid id, UserUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
