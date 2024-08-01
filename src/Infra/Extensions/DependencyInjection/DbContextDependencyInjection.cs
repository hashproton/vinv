using Infra.Extensions.DependencyInjection.Shared;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
namespace Infra.Extensions.DependencyInjection
{
    public class DatabaseConfiguration
    {
        [Required]
        public string ConnectionString { get; set; } = null!;
    }
    public static class DbContextDependencyInjection
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatedOptions<DatabaseConfiguration>(configuration, "Database");

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var dbConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
                options.UseSqlite(dbConfig.ConnectionString);
            });
        }
    }
}
