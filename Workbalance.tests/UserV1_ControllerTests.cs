using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Workbalance.Application.Dtos;
using Workbalance.Hateoas;
using Workbalance.Infrastructure.Context;
using Workbalance.tests;
using Xunit;

namespace Workbalance.tests
{
    public class UserV1_ControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public UserV1_ControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllV1_ShouldReturnUsers()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Users.Add(new Workbalance.Domain.Entity.User
                {
                    Id = Guid.NewGuid(),
                    Name = "Test User",
                    Email = "test@mail.com",
                    PasswordHash = "hashed",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                db.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/1.0/users");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<Resource<UserResponseDto>>>();
            Assert.NotNull(data);
            Assert.NotEmpty(data); // deve ter pelo menos 1 usuário
        }
    }
}
