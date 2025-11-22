using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Workbalance.Application.Swagger
{
    public class SwaggerConfigOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerConfigOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    desc.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"WorkBalance API {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString(),
                        Description = desc.IsDeprecated
                            ? "Esta versão da API está obsoleta."
                            : "API WorkBalance"
                    }
                );
            }
        }
    }
}
