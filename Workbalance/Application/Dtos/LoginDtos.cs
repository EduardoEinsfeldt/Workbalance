namespace Workbalance.Application.Dtos
{
    public record LoginDto(string Email, string Password);

    public record LoginResponseDto(string Token, Guid UserId, string Email);
}
