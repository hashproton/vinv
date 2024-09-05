using System.Net;
using System.Net.Http.Json;
using Application.Features.Commands.UpdateTenant;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;

namespace Presentation.Api.IntegrationTests.Endpoints.TenantEndpoints;

[TestClass]
public class UpdateTenantTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task ShouldReturnNoContent_WhenTenantIsUpdated()
    {
        // Arrange
        var existingTenant = new Domain.Tenant { Name = "Existing Tenant" };
        await TenantsRepository.CreateAsync(existingTenant, default);

        var command = new UpdateTenant.Command { Id = existingTenant.Id, Name = "Updated Tenant" };

        // Act
        var response = await Client.PutAsJsonAsync(Shared.Endpoints.Base, command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify in the database
        var updatedTenant = await TenantsRepository.GetByIdAsync(existingTenant.Id, default);
        updatedTenant.Should().NotBeNull();
        updatedTenant!.Name.Should().Be("Updated Tenant");
    }

    [TestMethod]
    public async Task ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var command = new UpdateTenant.Command { Id = 999, Name = "Nonexistent Tenant" };

        // Act
        var response = await Client.PutAsJsonAsync(Shared.Endpoints.Base, command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Tenant with id {command.Id} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task ShouldReturnConflict_WhenTenantNameAlreadyExists()
    {
        // Arrange
        var existingTenant1 = new Domain.Tenant { Name = "Tenant 1" };
        var existingTenant2 = new Domain.Tenant { Name = "Tenant 2" };
        await TenantsRepository.CreateManyAsync([existingTenant1, existingTenant2], default);

        var command = new UpdateTenant.Command { Id = existingTenant1.Id, Name = "Tenant 2" };

        // Act
        var response = await Client.PutAsJsonAsync(Shared.Endpoints.Base, command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.Conflict, $"Tenant with name {command.Name} already exists", ErrorType.AlreadyExists);
    }
}