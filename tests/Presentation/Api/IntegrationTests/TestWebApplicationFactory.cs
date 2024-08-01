using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Api.IntegrationTests;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public IServiceProvider ServiceProvider { get; set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.test.json");

            config.AddJsonFile(configPath);
        });

        builder.ConfigureServices(sp => ServiceProvider = sp.BuildServiceProvider());
    }

    public T GetService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}
