using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Workbalance.Application.Swagger
{
    public static class SwaggerSetup
    {
        public static void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.EnableAnnotations();

            // Documento para 1.0
            options.SwaggerDoc("1.0", new OpenApiInfo
            {
                Title = "WorkBalance API",
                Version = "1.0",
                Description = "API WorkBalance versão 1.0 (CRUD via EF)"
            });

            // Documento para 2.0
            options.SwaggerDoc("2.0", new OpenApiInfo
            {
                Title = "WorkBalance API",
                Version = "2.0",
                Description = "API WorkBalance versão 2.0 (procedures Oracle)"
            });

            // Filtra controladores pelas versões 1.0 / 2.0
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                var versions = apiDesc.ActionDescriptor.EndpointMetadata
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                // docName será "1.0" ou "2.0"
                return versions.Any(v => v.ToString() == docName);
            });
        }
    }
}
