using Application.Features.Queries.GetTenantById;

namespace Presentation.Api.IntegrationTests.Endpoints.TenantEndpoints;

[TestClass]
public class GetTenantByIdTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task ShouldReturnOk_WhenTenantExists()
    {
        // Arrange
        var existingTenant = new Tenant { Name = "Existing Tenant" };
        await TenantsRepository.CreateAsync(existingTenant, default);

        // Act
        var response = await Client.GetAsync($"{Shared.Endpoints.Base}/{existingTenant.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tenant = await response.Content.ReadFromJsonAsync<GetTenantById.TenantDto>();
        tenant.Should().NotBeNull();
        tenant!.Id.Should().Be(existingTenant.Id);
        tenant.Name.Should().Be(existingTenant.Name);
    }

    [TestMethod]
    public async Task ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var nonExistentTenantId = 999;

        // Act
        var response = await Client.GetAsync($"{Shared.Endpoints.Base}/{nonExistentTenantId}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Tenant with id {nonExistentTenantId} not found", ErrorType.NotFound);
    }
}