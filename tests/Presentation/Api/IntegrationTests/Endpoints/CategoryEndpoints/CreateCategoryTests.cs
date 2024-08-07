using Application.Features.Commands.CreateCategory;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints.CategoryEndpoints;

[TestClass]
public class CreateCategoryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task CreateCategory_ShouldReturnBadRequest_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new CreateCategory.Command { Name = new string('a', 201) };

        // Act
        var response = await Client.PostAsJsonAsync("/api/categories", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "The length of 'Name' must be 200 characters or fewer. You entered 201 characters.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task CreateCategory_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new CreateCategory.Command { Name = "Test Category" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/categories", command);

        // Assert
        var responseData = await response.Content.ReadFromJsonAsync<CreateCategory.Response>();
        responseData.Should().NotBeNull();
        responseData!.Id.Should().BeGreaterThan(0);

        // Verify in the database
        var category = await CategoriesRepository.GetByIdAsync(responseData.Id, default);
        category.Should().NotBeNull();
        category!.Name.Should().Be("Test Category");
    }

    [TestMethod]
    public async Task CreateCategory_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateCategory.Command { Name = "" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/categories", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Name' must not be empty.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task CreateCategory_ShouldReturnConflict_WhenCategoryAlreadyExists()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var command = new CreateCategory.Command { Name = "Existing Category" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/categories", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.Conflict, "Category already exists", ErrorType.AlreadyExists);
    }
}