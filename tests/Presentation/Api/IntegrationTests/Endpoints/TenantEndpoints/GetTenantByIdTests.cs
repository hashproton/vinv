using System.Net;
using System.Net.Http.Json;
using Application.Features.Queries.GetCategoryById;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;

namespace Presentation.Api.IntegrationTests.Endpoints.TenantEndpoints;

[TestClass]
public class GetTenantByIdTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task ShouldReturnOk_WhenTenantExists()
    {
        // Arrange
        var existingTenant = new Domain.Tenant { Name = "Existing Category" };
        await TenantsRepository.CreateAsync(existingTenant, default);

        // Act
        var response = await Client.GetAsync($"{Shared.Endpoints.Base}/{existingTenant.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var category = await response.Content.ReadFromJsonAsync<GetCategoryById.CategoryDto>();
        category.Should().NotBeNull();
        category!.Id.Should().Be(existingTenant.Id);
        category.Name.Should().Be(existingTenant.Name);
    }

    [TestMethod]
    public async Task ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = 999;

        // Act
        var response = await Client.GetAsync($"{Shared.Endpoints.Base}/{nonExistentCategoryId}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Tenant with id {nonExistentCategoryId} not found", ErrorType.NotFound);
    }
}