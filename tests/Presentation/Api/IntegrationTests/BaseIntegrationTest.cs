using Infra.Repositories;

namespace Presentation.Api.IntegrationTests;

[TestClass]
public abstract class BaseIntegrationTest
{
    protected TestWebApplicationFactory<Program> Factory { get; private set; } = null!;
    protected HttpClient Client { get; private set; } = null!;
    protected AppDbContext Context { get; private set; } = null!;

    [TestInitialize]
    public virtual void TestInitialize()
    {
        Factory = new TestWebApplicationFactory<Program>();
        Client = Factory.CreateClient();
        Context = Factory.GetService<AppDbContext>();
        Context.Database.EnsureCreated();
    }

    [TestCleanup]
    public virtual void TestCleanup()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
        Client.Dispose();
        Factory.Dispose();
    }
}