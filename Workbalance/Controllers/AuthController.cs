using Microsoft.AspNetCore.Mvc;
using Workbalance.Application.Dtos;
using Workbalance.Application.JWT;
using Workbalance.Infrastructure.Repository;
using Workbalance.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Workbalance.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/{version:apiVersion}/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _repo;
        private readonly JwtTokenService _jwt;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthController(IRepository<User> repo, JwtTokenService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var users = await _repo.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
                return Unauthorized("Email ou senha inválidos.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Email ou senha inválidos.");

            var token = _jwt.GenerateToken(user);

            return Ok(new LoginResponseDto(token, user.Id, user.Email));
        }
    }
}
