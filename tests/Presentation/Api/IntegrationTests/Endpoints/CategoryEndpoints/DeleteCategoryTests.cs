using Application.Errors;
using Application.Features.Commands.DeleteCategory;

namespace Presentation.Api.IntegrationTests.Endpoints.CategoryEndpoints;

[TestClass]
public class DeleteCategoryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task DeleteCategory_ShouldReturnNoContent_WhenCategoryIsDeleted()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        // Act
        var response = await Client.DeleteAsync($"/api/categories/{existingCategory.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var category = await CategoriesRepository.GetByIdAsync(existingCategory.Id, default);
        category.Should().BeNull();
    }

    [TestMethod]
    public async Task DeleteCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new DeleteCategory.Command { Id = 999 };

        // Act
        var response = await Client.DeleteAsync($"/api/categories/{command.Id}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, CategoryErrors.NotFoundById(command.Id));
    }

    [TestMethod]
    public async Task DeleteCategory_ShouldReturnBadRequest_WhenIdIsZero()
    {
        // Arrange
        var command = new DeleteCategory.Command { Id = 0 };

        // Act
        var response = await Client.DeleteAsync($"/api/categories/{command.Id}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Id' must be greater than '0'.", ErrorType.ValidationError);
    }
}