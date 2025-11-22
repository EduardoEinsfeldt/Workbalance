using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Workbalance.Infrastructure.Context;
using Workbalance.Hateoas;

namespace Workbalance.tests
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Remover Oracle
                var oracleDescriptor = services
                    .SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (oracleDescriptor != null)
                    services.Remove(oracleDescriptor);

                // Remover AppDbContext original
                var dbContextDescriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));

                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                // Registrar DbContext InMemory
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // 🔥 Registrar o LinkBuilder (necessário para DI)
                services.AddScoped<LinkBuilder>();

                // Criar banco em memória
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
