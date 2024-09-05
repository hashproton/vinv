using Application.Features.Queries.GetCategoryById;

namespace Presentation.Api.IntegrationTests.Endpoints.CategoryEndpoints;

[TestClass]
public class GetCategoryByIdTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetCategoryById_ShouldReturnOk_WhenCategoryExists()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        // Act
        var response = await Client.GetAsync($"/api/categories/{existingCategory.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var category = await response.Content.ReadFromJsonAsync<GetCategoryById.CategoryDto>();
        category.Should().NotBeNull();
        category!.Id.Should().Be(existingCategory.Id);
        category.Name.Should().Be(existingCategory.Name);
    }

    [TestMethod]
    public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = 999;

        // Act
        var response = await Client.GetAsync($"/api/categories/{nonExistentCategoryId}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Category with id {nonExistentCategoryId} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task GetCategoryById_ShouldReturnBadRequest_WhenIdIsZero()
    {
        // Arrange
        var query = new GetCategoryById.Query { Id = 0 };

        // Act
        var response = await Client.GetAsync($"/api/categories/{query.Id}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Id' must be greater than '0'.", ErrorType.ValidationError);
    }
}