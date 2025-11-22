using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using HealthChecks.UI.Client;
using Workbalance.Hateoas;
using Workbalance.Infrastructure.Context;
using Workbalance.Infrastructure.Repository;
using Workbalance.Application.JWT;
using Workbalance.Application.Swagger;
using Workbalance.Application.Services.Users;
using Workbalance.Application.Services.MoodEntries;
using Workbalance.Application.Services.Recommendations;

var builder = WebApplication.CreateBuilder(args);

// Database
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var cs = builder.Configuration.GetConnectionString("Oracle")
            ?? throw new InvalidOperationException("Connection string 'Oracle' is missing.");
        options.UseOracle(cs);
    });
}

// JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<JwtTokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
        };
    });

builder.Services.AddAuthorization();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader()
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigOptions>();

builder.Services.AddSwaggerGen(options =>
{
    SwaggerSetup.ConfigureSwagger(options);
    options.SupportNonNullableReferenceTypes();

    // JWT no Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticação JWT utilizando Bearer. Exemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

// Services
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<UserServiceV1>();
builder.Services.AddScoped<UserServiceV2>();
builder.Services.AddScoped<MoodEntryServiceV1>();
builder.Services.AddScoped<MoodEntryServiceV2>();
builder.Services.AddScoped<RecommendationServiceV1>();
builder.Services.AddScoped<RecommendationServiceV2>();
builder.Services.AddScoped<LinkBuilder>();

// HealthCheck
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

// Build
var app = builder.Build();
var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Swagger UI
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    foreach (var desc in apiVersionProvider.ApiVersionDescriptions)
    {
        var group = desc.ApiVersion.ToString();
        options.SwaggerEndpoint($"/swagger/{group}/swagger.json", $"WorkBalance API {group}");
    }

    options.RoutePrefix = "swagger";
});

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

// Health Check
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program { }
