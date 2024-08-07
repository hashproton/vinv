using Application.Features.Commands.UpdateCategory;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints.CategoryEndpoints;

[TestClass]
public class UpdateCategoryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task UpdateCategory_ShouldReturnNoContent_WhenCategoryIsUpdated()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var command = new UpdateCategory.Command { Id = existingCategory.Id, Name = "Updated Category" };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/categories/", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify in the database
        var updatedCategory = await CategoriesRepository.GetByIdAsync(existingCategory.Id, default);
        updatedCategory.Should().NotBeNull();
        updatedCategory!.Name.Should().Be("Updated Category");
    }

    [TestMethod]
    public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new UpdateCategory.Command { Id = 999, Name = "Nonexistent Category" };

        // Act
        var response = await Client.PutAsJsonAsync("/api/categories/", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Category with id {command.Id} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task UpdateCategory_ShouldReturnBadRequest_WhenIdIsZero()
    {
        // Arrange
        var command = new UpdateCategory.Command { Id = 0, Name = "Invalid Id Category" };

        // Act
        var response = await Client.PutAsJsonAsync("/api/categories/", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Id' must be greater than '0'.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task UpdateCategory_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var command = new UpdateCategory.Command { Id = existingCategory.Id, Name = "" };

        // Act
        var response = await Client.PutAsJsonAsync("/api/categories/", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Name' must not be empty.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task UpdateCategory_ShouldReturnConflict_WhenCategoryNameAlreadyExists()
    {
        // Arrange
        var existingCategory1 = new Domain.Category { Name = "Category 1" };
        var existingCategory2 = new Domain.Category { Name = "Category 2" };
        await CategoriesRepository.CreateManyAsync([existingCategory1, existingCategory2], default);

        var command = new UpdateCategory.Command { Id = existingCategory1.Id, Name = "Category 2" };

        // Act
        var response = await Client.PutAsJsonAsync("/api/categories/", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.Conflict, $"Category with name {command.Name} already exists", ErrorType.AlreadyExists);
    }
}