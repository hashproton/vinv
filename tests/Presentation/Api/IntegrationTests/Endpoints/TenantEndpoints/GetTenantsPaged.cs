using Application.Features.Queries.GetTenantsPaged;
using Application.Repositories.Shared;

namespace Presentation.Api.IntegrationTests.Endpoints.TenantEndpoints;

[TestClass]
public class GetTenantsPagedTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task ShouldReturnPagedResults_WhenValidRequestIsMade()
    {
        // Arrange
        var tenants = new List<Domain.Tenant>
        {
            new() { Name = "Tenant 1" },
            new() { Name = "Tenant 2" },
            new() { Name = "Tenant 3" },
            new() { Name = "Tenant 4" },
            new() { Name = "Tenant 5" }
        };
        foreach (var tenant in tenants)
        {
            await TenantsRepository.CreateAsync(tenant, default);
        }

        // Act
        var response = await Client.GetAsync($"{Shared.Endpoints.Base}?skip=1&take=2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<GetTenantsPaged.TenantDto>>();
        result.Should().NotBeNull();
        result!.Skip.Should().Be(1);
        result.Take.Should().Be(2);
        result.Total.Should().Be(5);
        result.Items.Should().HaveCount(2);
        result.Items.First().Name.Should().Be("Tenant 2");
        result.Items.Last().Name.Should().Be("Tenant 3");
    }

    [TestMethod]
    public async Task ShouldReturnFilteredResults_WhenFilterIsProvided()
    {
        // Arrange
        var tenants = new List<Domain.Tenant>
        {
            new() { Name = "Alpha Tenant" },
            new() { Name = "Beta Tenant" },
            new() { Name = "Gamma Tenant" }
        };
        foreach (var tenant in tenants)
        {
            await TenantsRepository.CreateAsync(tenant, default);
        }

        // Act
        var filter = Uri.EscapeDataString(""" Name @= "Alpha" """);
        var response = await Client.GetAsync($"{Shared.Endpoints.Base}?filter={filter}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<GetTenantsPaged.TenantDto>>();
        result.Should().NotBeNull();
        result!.Total.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Be("Alpha Tenant");
    }

    [TestMethod]
    public async Task ShouldReturnEmptyResult_WhenNoTenantsExist()
    {
        // Act
        var response = await Client.GetAsync(Shared.Endpoints.Base);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<GetTenantsPaged.TenantDto>>();
        result.Should().NotBeNull();
        result!.Total.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
}