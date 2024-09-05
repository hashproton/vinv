using Application.Features.Commands.CreateTenant;

namespace Presentation.Api.IntegrationTests.Endpoints.TenantEndpoints;

[TestClass]
public class CreateTenantTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task ShouldReturnCreatedResult_WhenTenantIsNew()
    {
        // Arrange
        var command = new CreateTenant.Command
        {
            Name = "Test Tenant",
            Status = TenantStatus.Demo
        };

        // Act
        var response = await Client.PostAsJsonAsync(Shared.Endpoints.Base, command);

        // Assert
        var responseData = await response.Content.ReadFromJsonAsync<CreateTenant.Response>();
        responseData.Should().NotBeNull();
        responseData!.Id.Should().BeGreaterThan(0);

        // Verify in the database
        var tenant = await TenantsRepository.GetByIdAsync(responseData.Id, default);
        tenant.Should().NotBeNull();
        tenant!.Name.Should().Be("Test Tenant");
        tenant.Status.Should().Be(TenantStatus.Demo);
    }

    [TestMethod]
    public async Task ShouldReturnConflict_WhenTenantAlreadyExists()
    {
        // Arrange
        var existingTenant = new Tenant { Name = "Existing Tenant" };
        await TenantsRepository.CreateAsync(existingTenant, default);

        var command = new CreateTenant.Command { Name = "Existing Tenant" };

        // Act
        var response = await Client.PostAsJsonAsync(Shared.Endpoints.Base, command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.Conflict, "Tenant already exists", ErrorType.AlreadyExists);
    }
}