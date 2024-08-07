using Infra.Extensions.DependencyInjection.Shared;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Infra.Extensions.DependencyInjection;

public class DatabaseConfiguration
{
    public enum DatabaseType
    {
        Sqlite,
        Postgres
    }

    [Required]
    public required string ConnectionString { get; init; }

    [Required]
    public DatabaseType Provider { get; set; }
}

public static class DbContextDependencyInjection
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatedOptions<DatabaseConfiguration>(configuration, "Database");

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

            options.SetDatabaseProvider(dbConfig.Provider, dbConfig.ConnectionString);

            options.UseSqlite(dbConfig.ConnectionString);
        });
    }

    private static DbContextOptionsBuilder SetDatabaseProvider(
        this DbContextOptionsBuilder options,
        DatabaseConfiguration.DatabaseType provider,
        string connectionString)
    {
        return provider switch
        {
            DatabaseConfiguration.DatabaseType.Postgres => options.UseNpgsql(connectionString),
            DatabaseConfiguration.DatabaseType.Sqlite => options.UseSqlite(connectionString),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    }
}