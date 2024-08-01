using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infra.Extensions.DependencyInjection.Shared
{
    public static class OptionsExtensions
    {
        public static OptionsBuilder<TOptions> AddValidatedOptions<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName) where TOptions : class
        {
            return services.AddOptions<TOptions>()
                .Bind(configuration.GetSection(sectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
