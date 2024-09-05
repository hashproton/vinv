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
    public DatabaseType Provider { get; set; }
    
    [Required]
    public required DatabaseOptions Options { get; init; }

    public class DatabaseOptions
    {
        public class PostgresOptions
        {
            [Required]
            public required string ConnectionString { get; init; }
        }

        public class SqliteOptions
        {
            [Required]
            public required string ConnectionString { get; init; }
        }

        public PostgresOptions? Postgres { get; init; }
        public SqliteOptions? Sqlite { get; init; }
    }
}

public static class DbContextDependencyInjection
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatedOptions<DatabaseConfiguration>(configuration, "Database");

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

            options.SetDatabaseProvider(dbConfig);
        });
    }

    private static DbContextOptionsBuilder SetDatabaseProvider(
        this DbContextOptionsBuilder options,
        DatabaseConfiguration dbConfig)
    {
        var connectionString = dbConfig.Provider switch
        {
            DatabaseConfiguration.DatabaseType.Postgres => dbConfig.Options?.Postgres?.ConnectionString,
            DatabaseConfiguration.DatabaseType.Sqlite => dbConfig.Options?.Sqlite?.ConnectionString,
        };

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string for {dbConfig.Provider} is missing or empty.");
        }

        var test = InfraReference.Assembly.GetName().Name;

        return dbConfig.Provider switch
        {
            DatabaseConfiguration.DatabaseType.Postgres => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(InfraReference.Assembly.GetName().Name)),
            DatabaseConfiguration.DatabaseType.Sqlite => options.UseSqlite(connectionString, b => b.MigrationsAssembly(InfraReference.Assembly.GetName().Name)),
        };
    }
}