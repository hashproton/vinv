namespace Presentation.Api.IntegrationTests.Endpoints.TenantEndpoints;

[TestClass]
public class DeleteTenantTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task ShouldReturnNoContent_WhenTenantIsDeleted()
    {
        // Arrange
        var existingTenant = new Domain.Tenant { Name = "Existing Tenant" };
        await TenantsRepository.CreateAsync(existingTenant, default);

        // Act
        var response = await Client.DeleteAsync($"{Shared.Endpoints.Base}/{existingTenant.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify in the database
        var deletedProduct = await ProductsRepository.GetByIdAsync(existingTenant.Id, default);
        deletedProduct.Should().BeNull();
    }

    [TestMethod]
    public async Task ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var nonExistentProductId = 999;

        // Act
        var response = await Client.DeleteAsync($"{Shared.Endpoints.Base}/{nonExistentProductId}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Tenant with id {nonExistentProductId} not found", ErrorType.NotFound);
    }
}
