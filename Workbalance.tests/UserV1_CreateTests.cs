using System.Net;
using System.Net.Http.Json;
using Workbalance.Application.Dtos;
using Xunit;

namespace Workbalance.tests
{
    public class UserV1_CreateTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UserV1_CreateTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateUserV1_ShouldReturn201()
        {
            // Arrange
            var dto = new UserCreateDto(
                Name: "User Test",
                Email: $"test_{Guid.NewGuid()}@mail.com",
                Password: "123456",
                PreferredLanguage: "pt-BR"
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/1.0/users", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
