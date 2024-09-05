namespace Presentation.Api.IntegrationTests.Endpoints.CategoryEndpoints;

[TestClass]
public class RemoveProductFromCategoryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task RemoveProductFromCategory_ShouldReturnNoContent_WhenProductIsRemovedFromCategory()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Product to Remove", CategoryId = existingCategory.Id };
        existingCategory.Products.Add(existingProduct);
        await ProductsRepository.CreateAsync(existingProduct, default);
        await CategoriesRepository.UpdateAsync(existingCategory, default);

        // Act
        var response = await Client.DeleteAsync($"/api/categories/{existingCategory.Id}/products/{existingProduct.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify in the database
        var updatedCategory = await CategoriesRepository.GetByIdAsync(existingCategory.Id, default);
        updatedCategory.Should().NotBeNull();
        updatedCategory!.Products.Should().NotContain(p => p.Id == existingProduct.Id);
    }

    [TestMethod]
    public async Task RemoveProductFromCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Product to Remove", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        // Act
        var response = await Client.DeleteAsync($"/api/categories/999/products/{existingProduct.Id}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Category with id 999 not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task RemoveProductFromCategory_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        // Act
        var response = await Client.DeleteAsync($"/api/categories/{existingCategory.Id}/products/999");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Product with id 999 not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task RemoveProductFromCategory_ShouldReturnBadRequest_WhenProductIdIsZero()
    {
        // Arrange

        // Act
        var response = await Client.DeleteAsync($"/api/categories/1/products/0");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Product Id' must be greater than '0'.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task RemoveProductFromCategory_ShouldReturnBadRequest_WhenCategoryIdIsZero()
    {
        // Arrange

        // Act
        var response = await Client.DeleteAsync($"/api/categories/0/products/1");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Category Id' must be greater than '0'.", ErrorType.ValidationError);
    }
}
