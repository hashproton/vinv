using Application.Repositories;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Api.IntegrationTests;

[TestClass]
public abstract class BaseIntegrationTest : IAsyncDisposable
{
    private readonly TestWebApplicationFactory<Program> _factory = new();
    private IServiceScope? _scope;
    private AppDbContext Context => GetScopedService<AppDbContext>();

    protected HttpClient Client { get; private set; } = null!;
    protected ICategoriesRepository CategoriesRepository => GetScopedService<ICategoriesRepository>();


    private TService GetScopedService<TService>() where TService : notnull
    {
        _scope?.Dispose();
        _scope = _factory.Services.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<TService>();
    }

    private async Task ResetDatabaseAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.Database.EnsureCreatedAsync();
    }

    [TestInitialize]
    public virtual async Task TestInitializeAsync()
    {
        Client = _factory.CreateClient();
        await ResetDatabaseAsync();
    }

    [TestCleanup]
    public virtual async Task TestCleanupAsync()
    {
        await Context.Database.EnsureDeletedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    private async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_scope);
        await _factory.DisposeAsync();
        await CastAndDispose(Client);
    }

    private static async ValueTask CastAndDispose(IDisposable? resource)
    {
        if (resource is IAsyncDisposable resourceAsyncDisposable)
        {
            await resourceAsyncDisposable.DisposeAsync();
        }
        else
        {
            resource?.Dispose();
        }
    }
}